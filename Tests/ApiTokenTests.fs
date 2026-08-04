module FsFs.Tests.ApiTokenTests

open Xunit
open FsFs.Infrastructure.Config
open FsFs.Infrastructure.Middleware

[<Fact>]
let ``parseApiTokens splits and trims`` () =
    Assert.Equal<string list>([ "a"; "b"; "c" ], parseApiTokens " a , b,c ")

[<Fact>]
let ``parseApiTokens empty yields empty list`` () =
    Assert.Equal<string list>([], parseApiTokens "")
    Assert.Equal<string list>([], parseApiTokens "  , , ")

[<Fact>]
let ``tryGetBearerToken extracts token`` () =
    Assert.Equal(Some "secret", tryGetBearerToken (Some "Bearer secret"))
    Assert.Equal(Some "secret", tryGetBearerToken (Some "bearer secret"))

[<Fact>]
let ``tryGetBearerToken rejects missing or malformed`` () =
    Assert.Equal(None, tryGetBearerToken None)
    Assert.Equal(None, tryGetBearerToken (Some "Basic x"))
    Assert.Equal(None, tryGetBearerToken (Some "Bearer "))

[<Fact>]
let ``isApiTokenAllowed checks list membership`` () =
    Assert.True(isApiTokenAllowed [ "a"; "b" ] "a")
    Assert.False(isApiTokenAllowed [ "a"; "b" ] "c")
    Assert.False(isApiTokenAllowed [] "a")
