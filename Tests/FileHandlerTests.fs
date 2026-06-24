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
