module FsFs.Tests.FileHandlerTests

open System.IO
open Xunit
open FsFs.Handlers.FileHandler

[<Fact>]
let ``safeDeleteFile returns FileNotFound when file does not exist`` () =
    let missingPath = Path.Combine(Path.GetTempPath(), "fsfs-missing-file.txt")

    let result = safeDeleteFile missingPath

    match result with
    | Error(FileNotFound path) -> Assert.Equal(missingPath, path)
    | other -> Assert.Fail($"Expected FileNotFound, got {other}")

[<Fact>]
let ``safeDeleteFile deletes an existing file`` () =
    let tempFile = Path.GetTempFileName()

    try
        let result = safeDeleteFile tempFile

        match result with
        | Ok() -> Assert.False(File.Exists tempFile)
        | other -> Assert.Fail($"Expected Ok, got {other}")
    finally
        if File.Exists tempFile then
            File.Delete tempFile

[<Fact>]
let ``safeGetFiles rejects path traversal with slash`` () =
    match safeGetFiles (Path.GetTempPath()) "a/b" with
    | Error msg -> Assert.Equal("Invalid path", msg)
    | Ok _ -> Assert.Fail("Expected Invalid path")

[<Fact>]
let ``safeGetFiles rejects path traversal with dotdot`` () =
    match safeGetFiles (Path.GetTempPath()) ".." with
    | Error msg -> Assert.Equal("Invalid path", msg)
    | Ok _ -> Assert.Fail("Expected Invalid path")

[<Fact>]
let ``safeGetFiles returns relative paths for files in subdirectory`` () =
    let root = Path.Combine(Path.GetTempPath(), $"fsfs-list-{System.Guid.NewGuid():N}")
    let sub = Path.Combine(root, "resource")
    Directory.CreateDirectory sub |> ignore
    let filePath = Path.Combine(sub, "note.txt")
    File.WriteAllText(filePath, "hello")

    try
        match safeGetFiles root "resource" with
        | Ok files ->
            Assert.Contains(files, fun f -> f.Replace("\\", "/").EndsWith("resource/note.txt"))
        | Error msg -> Assert.Fail($"Expected Ok, got {msg}")
    finally
        if Directory.Exists root then
            Directory.Delete(root, true)

[<Fact>]
let ``safeGetAllFiles collects files from child directories`` () =
    let root = Path.Combine(Path.GetTempPath(), $"fsfs-all-{System.Guid.NewGuid():N}")
    let sub = Path.Combine(root, "docs")
    Directory.CreateDirectory sub |> ignore
    File.WriteAllText(Path.Combine(sub, "a.txt"), "a")

    try
        match safeGetAllFiles root with
        | Ok files ->
            Assert.True(files.Length >= 1)
            Assert.Contains(files, fun f -> f.Replace("\\", "/").EndsWith("docs/a.txt"))
        | Error msg -> Assert.Fail($"Expected Ok, got {msg}")
    finally
        if Directory.Exists root then
            Directory.Delete(root, true)
