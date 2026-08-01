module FsFs.Tests.CreateDirectoryHandlerTests

open System.IO
open Xunit
open FsFs.Handlers.CreateDirectoryHandler

[<Fact>]
let ``normalizeDirName strips whitespace`` () =
    Assert.Equal("docs", normalizeDirName "  d o c s  ")

[<Fact>]
let ``validateDirName rejects slash and dotdot`` () =
    match validateDirName "a/b" with
    | Error InvalidPath -> ()
    | other -> Assert.Fail($"Expected InvalidPath, got {other}")

    match validateDirName ".." with
    | Error InvalidPath -> ()
    | other -> Assert.Fail($"Expected InvalidPath, got {other}")

[<Fact>]
let ``validateDirName rejects empty name`` () =
    match validateDirName "" with
    | Error EmptyName -> ()
    | other -> Assert.Fail($"Expected EmptyName, got {other}")

[<Fact>]
let ``validateDirName accepts simple name`` () =
    match validateDirName "resource" with
    | Ok name -> Assert.Equal("resource", name)
    | Error e -> Assert.Fail($"Expected Ok, got {e}")

[<Fact>]
let ``tryCreateDirectory creates missing directory`` () =
    let root = Path.Combine(Path.GetTempPath(), $"fsfs-mkdir-{System.Guid.NewGuid():N}")
    Directory.CreateDirectory root |> ignore

    try
        match tryCreateDirectory root "newfolder" with
        | Created -> Assert.True(Directory.Exists(Path.Combine(root, "newfolder")))
        | AlreadyExists -> Assert.Fail("Expected Created")
    finally
        if Directory.Exists root then
            Directory.Delete(root, true)

[<Fact>]
let ``tryCreateDirectory returns AlreadyExists without failing`` () =
    let root = Path.Combine(Path.GetTempPath(), $"fsfs-mkdir2-{System.Guid.NewGuid():N}")
    let existing = Path.Combine(root, "keep")
    Directory.CreateDirectory existing |> ignore

    try
        match tryCreateDirectory root "keep" with
        | AlreadyExists -> Assert.True(Directory.Exists existing)
        | Created -> Assert.Fail("Expected AlreadyExists")
    finally
        if Directory.Exists root then
            Directory.Delete(root, true)
