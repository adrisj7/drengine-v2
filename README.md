# drengine-v2

## CURRENTLY DISCONTINUED, see https://github.com/adrisj7/dang-engine for new project that runs in the web

A Fanganronpa creator. Let the fan chaos commence.

TODO: Give the folks at home a description on what the heck this is.

There are no releases (yet), so you'll have to run the project from source. See Build guide below.

## Notion/TODO Board

[Link to Board](https://www.notion.so/a75c5a9872bc4f0c8f227c7b7ea91cac?v=1284754e15634900a7c42a6e2e539ad7)

## How to Build the Project

1. Download the folder and open the sln with Jetbrains Rider (preferred) or Visual Studio (untested but should work).
2. You probably need to import missing libraries. Check out the [Library Setup README](https://github.com/adrisj7/drengine-v2/blob/main/GameEngine/libs/README.md).
In the future this might be simplified but for now you'll have to follow the README guide.
3. That should be all. To run the engine just run the binary executable from the "DR Engine v2" project. To run the game add `--game` as a argument to the executable.

Linux users: If you have dotnet/msbuild installed I have a Makefile that you can use. `make run_engine` and `make run_game` should do the trick.
