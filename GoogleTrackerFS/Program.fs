open System
open System.Text.RegularExpressions
open HtmlAgilityPack
open System.Net

let possibly f a = try f a |> Some with _ -> None
let alternatively v o = defaultArg o v

type SearchInstance = { 
    Retreived: DateTime; Query: string; Count: int64 
}

type Result = { 
    Url: string; Title: string; Description: string; SearchPageNo: int; Instance: SearchInstance 
}

let GoogleQueryUrl = "http://google.co.uk/search?q="

let sanitiseQuery query = Regex.Replace(query, "[ _]", "+")

let extractSearchInstance (query: string) (document: HtmlDocument) = 
    let count = 
        document.GetElementbyId("resultStats").InnerText.Split()
        |> Seq.tryPick (fun w -> w.Replace(",", "") |> possibly int64)
        |> alternatively 0L 
    { Retreived = DateTime.Now; Query = query; Count = count }

let resultFromRaw (instance: SearchInstance) (node: HtmlNode) =
    let extract xp = node.SelectSingleNode(xp).InnerText
    let url, title, description = 
        extract ".//cite", extract ".//h3[@class='r']", extract ".//span[@class='st']"; 
    { Url = url; Title = title; Description = description; SearchPageNo = 1; Instance = instance }

let getSearchResults query =
    let document = HtmlDocument()
    WebRequest.Create(GoogleQueryUrl + (sanitiseQuery query)).GetResponse().GetResponseStream()
    |> document.Load
    let instance = document |> extractSearchInstance query
    document.DocumentNode.SelectNodes(".//div[@class='g']")
    |> Seq.choose (possibly (resultFromRaw instance))

[<EntryPoint>]
let main argv = 
    "Flying Colors" |> getSearchResults |> Seq.length |> printfn "%d"
    Console.ReadLine() |> ignore
    0
