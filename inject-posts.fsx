open System

type Metadata = {
    title: string
    date: string
    tags: string list
}

let template meta file = 
    let tagSpans = 
        match meta.tags with
        | [] -> ""
        | _ -> 
            let spans = 
                meta.tags 
                |> List.map (fun tag -> $"""<span class="tag">{tag}</span>""") 
                |> String.concat ""
            $"""<div class="tags">{spans}</div>"""
    
    $"""
<div class="timeline-item">
    <a href="{file}">
        <div class="content">
            <p class="commit-message">{meta.title}</p>
            <p class="commit-date">{meta.date}</p>
            {tagSpans}
        </div>
    </a>
</div>"""



let getMetadata (file: string) =
    let meta =
        IO.File.ReadAllLines file
        |> Array.skipWhile (fun line -> String.IsNullOrWhiteSpace line || line = "---")
        |> Array.takeWhile ((<>) "---")
        |> Array.choose (fun line ->
            let split = line.Split ": "
            (Array.tryHead split, Array.tryItem 1 split)
            ||> Option.map2 (fun key value -> key, value)
        )

    let get key =
        Array.tryFind (fst >> (=) key)
        >> Option.map snd 
        >> Option.defaultWith (fun () -> failwithf "File %s is missing meta key %s" file key)

    let splitToList (str: string) = str.Split(" ", StringSplitOptions.RemoveEmptyEntries) |> Array.toList 

    {
        title = meta |> get "title"
        date = meta |> get "date"
        tags = meta |> get "tags" |> splitToList
    }

let replaceExtension (ext: string) (file: string) =
    file.Split "." |> Array.head |> fun name -> $"{name}.{ext}"

let content =
    System.IO.Directory.GetFiles "posts"
    |> Array.map (fun file -> file, getMetadata file)
    |> Array.sortByDescending (snd >> _.date)
    |> Array.map (fun (file, meta) -> replaceExtension "html" file |> template meta)
    |> String.concat "\n"

let index = System.IO.File.ReadAllText "index.html"
index.Replace("{timeline-items}", content)
|> fun html -> System.IO.File.WriteAllText("index.html", html)
