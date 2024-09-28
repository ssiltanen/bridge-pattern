---
title: First Blog Post
date: 2024-09-28
tags: First Gh-pages F# Jekyll Markdown
---

# {{ page.title }}

As the title proclaims this is my first blog post! I recently reached a milestone of working a full decade in the code mines we call IT industry and yet only just now I am publishing some of my thoughts. So what took me so long?

> There is already a plethora of IT blogs out there. What could I ever say that hasn't been said before?

What topic is worthy of the first blog post ever? The blank page syndrome or maybe in this case the empty blog syndrome is something this blog post set out to overcome. Maybe I don't have to bring about a new scientific discovery to make a blog post. Maybe in this case it is enought to talk about how this blog page was made, even though there are no new or hype technologies used.

## Bridge Pattern Blog

The main goals I wanted to achieve with this solution:

- Hosting is free
- Low maintenance
- Content is created as markdown files

Because the crypto miners ruined the free tier hosting services for all of us, this didn't leave me with many choices. I narrowed down on Github Pages since it is free and easy to configure. The downside is that Github Pages only supports static websites meaning no backend. This means that blog posts need to be rendered before hand. Additionally, to add a new blog post, the page would have to be redeployed. Luckily, Github pages is integrated out of the box with [Jekyll](https://jekyllrb.com/), which renders markdown files as html.

The setup requires

1. Enable Github pages for the repository
2. Add `_config.yml` for Jekyll to define theme for the rendered pages
3. Create blog posts

Now, Jekyll would support creating the whole blog page, but I wanted to create the langing page on my own. This added some extra steps for the setup.

1. Create a html template file `index.hml`
3. Setup Github Pages to use another branch, in this case `gh-pages`
2. Create a CI github action to find blog posts and inject anchors to them in the template index file. Finally push the injected html file and the blog posts to the `gh-pages` branch

Using a separate branch for the Github pages created a separation between the code and the runtime. This could be thought also as if building the solution and hosting it from the other branch.

The html injecting is done with an F# script (`.fsx`). F# script files do not require a project file like .NET things usually do. It can be invoked as is with F# interactive tool `dotnet fsi <file>`. The script file reads all files under `docs` and parses the [frontmatter](https://jekyllrb.com/docs/front-matter/) metadata properties for the index html. The metadata properties serve also as variables for the blog posts, which can be referenced in the text.

The github action checksout both branches (main and gh-pages), installs dotnet, executes the injection F# script, and commits the changed files into the gh-pages. A push to the gh-pages branch would then trigger the built-in github pages deployment where Jekyll would render the markdown files. Effectively, if nothing was changed in the gh-pages, no deployment is done.

### After thoughts

Using this setup turned out to be very simple and easy to make. My stubborness to make my own landing page created a little friction while making the solution, but its maintenance is relative simple. There are now 3rd party dependencies except for the actions used in CI. The landing page is plain html/css and adding new posts requires no code changes. As usually, the styling of the page probably will change over time, but at least it is in my hands.

The whole setup is public in Github so take a look if you are interested!

P.S. Hopefully the interval between the blog posts will be shorter next time.
