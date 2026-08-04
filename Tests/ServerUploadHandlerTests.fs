module FsFs.Tests.ServerUploadHandlerTests

open System
open System.IO
open Xunit
open FsFs.Handlers.ServerUploadHandler

[<Fact>]
let ``validateFileName accepts simple name`` () =
    match validateFileName "report.txt" with
    | Ok name -> Assert.Equal("report.txt", name)
    | Error e -> Assert.Fail($"Expected Ok, got {e}")

[<Fact>]
let ``validateFileName rejects path characters`` () =
    match validateFileName "a/b.txt" with
    | Error InvalidFileName -> ()
    | other -> Assert.Fail($"Expected InvalidFileName, got {other}")

    match validateFileName "..\\x.txt" with
    | Error InvalidFileName -> ()
    | other -> Assert.Fail($"Expected InvalidFileName, got {other}")

[<Fact>]
let ``validateFileName rejects blocked extensions`` () =
    for name in [ "a.js"; "b.EXE"; "c.dll"; "d.sh" ] do
        match validateFileName name with
        | Error BlockedExtension -> ()
        | other -> Assert.Fail($"Expected BlockedExtension for {name}, got {other}")

[<Fact>]
let ``decodeBase64 accepts valid payload`` () =
    let raw = Convert.ToBase64String([| 1uy; 2uy; 3uy |])

    match decodeBase64 raw with
    | Ok bytes -> Assert.Equal<byte[]>([| 1uy; 2uy; 3uy |], bytes)
    | Error e -> Assert.Fail($"Expected Ok, got {e}")

[<Fact>]
let ``decodeBase64 rejects invalid payload`` () =
    match decodeBase64 "not-base64!!!" with
    | Error InvalidBase64 -> ()
    | other -> Assert.Fail($"Expected InvalidBase64, got {other}")

[<Fact>]
let ``writeUploadBytes writes file and returns relative path`` () =
    let root = Path.Combine(Path.GetTempPath(), $"fsfs-server-up-{Guid.NewGuid():N}")
    let dir = Path.Combine(root, "uploads")
    Directory.CreateDirectory dir |> ignore

    try
        match writeUploadBytes root "uploads" "hello.txt" (System.Text.Encoding.UTF8.GetBytes "hi") with
        | Ok result ->
            Assert.True(File.Exists(Path.Combine(root, result.Path)))
            Assert.Contains("hello.txt", result.Path)
        | Error e -> Assert.Fail($"Expected Ok, got {e}")
    finally
        if Directory.Exists root then
            Directory.Delete(root, true)
