# SmallUrl

This web app is a conventional link shortener; it can convert long and messy URLs into links that are shorter and more readable. This project uses ASP.NET Core with Razor Pages and Entity Framework. I created this project primarily to teach myself ASP.NET Core.

You can try it out [here](http://smallurl.runasp.net/).

## Features

I’ve currently implemented the following:

- CRUD (Create, Read, Update, Delete) functionality for shortened links.
- Simple authentication and authorization.
  - Each shortened link can be tied to an authenticated user. Only the owner of a link can edit or delete it.
  - Links created by unauthenticated users are stored in the user’s cookies. Users can still edit or delete links that are stored in their cookies.
  - Authenticated users can claim ownership of links if those links were created during an unauthenticated session on the same browser/device.
- Redirection from the shortened link to the full-length URL. This is the core feature of every link shortener.
- Ability to generate and download QR codes for shortened links.

In the future, I may add link analytics as well.

## Running the Project Locally

You’ll need an installation of the Dotnet 9.0 SDK. [This page](https://learn.microsoft.com/en-us/dotnet/core/install/windows) from Microsoft’s documentation explains how to install it on Windows. The simplest way is by running the following in a command prompt:

```sh
winget install Microsoft.DotNet.SDK.9
```

Microsoft provides separate instructions for [Linux](https://learn.microsoft.com/en-us/dotnet/core/install/linux-scripted-manual) and [MacOS](https://learn.microsoft.com/en-us/dotnet/core/install/macos).

After installing the Dotnet SDK, you’ll need to install [Libman](https://learn.microsoft.com/en-us/aspnet/core/client-side/libman/?view=aspnetcore-9.0). Libman is a package manager for client-side dependencies in ASP.NET Core apps.

Libman can be installed by entering the following into a command prompt:

```sh
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
```

After installing the Dotnet SDK and Libman, run a `git clone` on this repository. From the root project directory, run the following:

```sh
libman restore
```

This will install all of the necessary client-side dependencies.

You can run SmallUrl inside Visual Studio, or use the `dotnet run` command from the root project directory.

## Deployment

I found [MonsterASP.net](https://www.monsterasp.net/) to be the most straightforward deployment option. To deploy the app with MonsterASP, I did the following:

1. Created an account with MonsterASP.
2. Created both a website and an MSSQL database through MonsterASP’s dashboard.
3. Obtained the (local, not remote) database connection string for the MSSQL database.
4. Created an environment variable for the website with a key of `DB_CONN`. I set the value of this environment variable to the full database connection string. Note that you can set environment variables in `Websites > Scripting` in MonsterASP’s dashboard.
5. Followed [this guide](https://help.monsterasp.net/books/github/page/how-to-deploy-website-via-github-actions) from MonsterASP's documentation. This involves adding secrets provided by MonsterASP into the GitHub repository, so you'll need to fork this repo if you'd like to deploy a copy yourself.

You can also run this project with Docker, but I ultimately did not end up using Docker for deployment.

Assuming that you have both Docker and Docker Compose installed, you can run the project with the following command:

```sh
docker compose up
```

If you want the project to handle requests on ports 80 and 443 (as might be common in a real application), you can run the following:

```sh
docker compose -f docker-compose.yml -f docker-compose-prod.yml up
```
