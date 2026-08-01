module FsFs.Tests.UploadHandlerTests

open System.IO
open Xunit
open FsFs.Handlers.UploadHandler

[<Fact>]
let ``validatePath accepts simple directory name`` () =
    match validatePath "resource" with
    | Ok name -> Assert.Equal("resource", name)
    | Error e -> Assert.Fail($"Expected Ok, got {e}")

[<Fact>]
let ``validatePath rejects slash`` () =
    match validatePath "a/b" with
    | Error InvalidPath -> ()
    | other -> Assert.Fail($"Expected InvalidPath, got {other}")

[<Fact>]
let ``validatePath rejects dotdot`` () =
    match validatePath ".." with
    | Error InvalidPath -> ()
    | other -> Assert.Fail($"Expected InvalidPath, got {other}")

[<Fact>]
let ``ensureDirectory returns Ok when directory exists`` () =
    let root = Path.Combine(Path.GetTempPath(), $"fsfs-up-{System.Guid.NewGuid():N}")
    let dir = Path.Combine(root, "uploads")
    Directory.CreateDirectory dir |> ignore

    try
        match ensureDirectory root "uploads" with
        | Ok fullPath -> Assert.Equal(Path.GetFullPath dir, Path.GetFullPath fullPath)
        | Error e -> Assert.Fail($"Expected Ok, got {e}")
    finally
        if Directory.Exists root then
            Directory.Delete(root, true)

[<Fact>]
let ``ensureDirectory returns DirectoryNotFound when missing`` () =
    let root = Path.Combine(Path.GetTempPath(), $"fsfs-up-missing-{System.Guid.NewGuid():N}")
    Directory.CreateDirectory root |> ignore

    try
        match ensureDirectory root "nope" with
        | Error DirectoryNotFound -> ()
        | other -> Assert.Fail($"Expected DirectoryNotFound, got {other}")
    finally
        if Directory.Exists root then
            Directory.Delete(root, true)
