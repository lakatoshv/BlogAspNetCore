namespace Blog.FSharp.Core.Infrastructure

open System
open System.IO
open System.Text
open System.Threading
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.FileProviders

type IShareFileProvider =
    inherit IFileProvider
    abstract Combine : string[] -> string
    abstract CreateDirectory : string -> unit
    abstract CreateFile : string -> unit
    abstract DeleteDirectory : string -> unit
    abstract DeleteFile : string -> unit
    abstract DirectoryExists : string -> bool
    abstract FileExists : string -> bool
    abstract FileLength : string -> int64
    abstract MapPath : string -> string
    abstract ReadAllText : string * Encoding -> string
    abstract WriteAllText : string * string * Encoding -> unit