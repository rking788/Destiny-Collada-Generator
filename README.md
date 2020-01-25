# Destiny Collada Generator
 A program to generate Collada files of items from Destiny 2

## Features:
 This program will, when complete, be capable of generating a 3D model of any item available through Destiny 2's web/mobile api and exporting said model in Collada format, with all data intact. Items on this list will be checked off as they are implemented.
- [X] Core functions
	- [X] Export geometry
	- [X] Export mesh weights
	- [X] Export UV texture coordinates
	- [X] Export per-face vertex normals and tangents
	- [X] Export vertex colors
	- [X] Export dye slots for compatibility with D2 model rippers' shaders

- [ ] API support
	- [X] Call files to convert from the API by item hash
	- [X] Convert files from the D1 API by item hash

- [ ] Additional features (lowest priority)
	- [ ] Generate textures for items in either .png or .dds format
	- [ ] Generate a list of shader parameters for items, in an easy-to-understand layout

 ## Error reporting: 
 If the program exits without warning (command line closes on its own in the middle of converting), it's likely due to a bug or error. If this happens, create a new issue on the github repo, including the TGXM file or item hash that caused the crash. I'll push out an updated version of the program once the issue is fixed.

 ## Usage:
 This tool is designed to be relatively simple to use. Just download it, unzip it, and run the executable. It will open a terminal window, and give you a list of options for what to do:
 ```
 Select an action:
 [1] Convert local files
 [2] Convert item from API
 [3] Convert item from D1 API
 [4] Quit
  > █
 ```
 Just enter the number of the action you want to do.

 *Note: Do not edit the "Resources" folder or the file inside, `template.dae`. Doing so will cause the tool to crash when generating the output file.*

 All converters will ask for an input and an output location. The input changes depending on which converter is being used, but the output location must be a valid location on your local system. If the folder given does not exist, it will be created. If no folder is given, it will default to the "Output" folder in the same location as the executable.

 ### Convert local files:
 This converter is mainly for debugging, but has been left in just in case. It takes a file location as an input. The file path can be relative (`input\4fe354c299526b2232b335b603756ad7.tgxm`) or absolute (`C:\Users\admin\Downloads\4fe354c299526b2232b335b603756ad7.tgxm`).

 ### Convert item from API / Convert item from D1 API:
 These converters takes item hashes as the input. Item hashes can be taken from sites such as Light.gg or db.destinytracker.com. Multiple hashes can be given, separated by spaces. If converting items from Destiny 2, use "Convert item from API". For items from Destiny, use "Convert item from D1 API".

 *Note: Since these use the web api, there are certain limits on what can be pulled. Map assets and enemies cannot be converted. Ghosts, ornaments, and sparrows from D1 cannot be converted, but D2 ones can. D2's season artifacts cannot be converted.*

 ### Preparing models for use:
 The models generated by this tool are not completely ready for use. They have many extra vertices, and loose edges link sections of the geometry together. For *most* models, this can be fixed rather quickly. If you are using Blender, follow these steps:
 1. Import the model.
 2. Go into edit mode and select all vertexes.
 3. Change selection mode to edges.
 4. Change selection mode back to vertexes.
 5. Invert selection (Ctrl+i by default).
 6. Delete vertexes. All loose vertexes should now be gone.
 7. Select all vertexes again.
 8. Separate by loose parts (P, then L by default). 
 9. Edit mode on all meshes. Select all vertexes.
 10. Change selection mode to faces.
 11. Change selection mode to edges.
 12. Invert selection.
 13. Delete edges.

 For some meshes, there will be no loose edges. To deal with these, the tool exports a weight group for each dye slot. Skip steps 7-13 above and instead find the vertex groups named "dyeSlot_[NAME#]". One group at a time, select the vertexes in these groups and separate by selection (P, then S by default).

 Once this is done, the models are ready for use. If necessary, meshes with the same dye slot that use the same texture set can be merged into one mesh.
 
 ## Code attributions:
 This program uses:

 Large sections of code adapted from [Lowlines' TGX loader for three.js](https://github.com/lowlines/destiny-tgx-loader), as well as their online manifest

 [Burtsev Alexey's .NET deep copy extension method](https://github.com/Burtsev-Alexey/net-object-deep-copy)

 [Alexandre Mutel's patch of the C# COLLADA specification classes](https://xoofx.com/blog/2010/08/24/import-and-export-3d-collada-files-with/)
