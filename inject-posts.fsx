open System

let template file = $"""
        <div class="timeline-item">
            <a href="{file}">
                <div class="content">
                    <p class="commit-message">Some Blog Post</p>
                    <p class="commit-date">2024-08-07</p>
                    <span class="tag">v1.1</span>
                </div>
            </a>
        </div>"""

let content =
    System.IO.Directory.GetFiles "posts"
    |> Array.map ((fun file -> file.Replace(".md", ".html")) >> template)
    |> String.concat "\n"

let index = System.IO.File.ReadAllText "index.html"
index.Replace("{timeline-items}", content)
|> fun html -> System.IO.File.WriteAllText("index.html", html)
