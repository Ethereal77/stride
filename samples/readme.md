Stride Samples
==============

* Each sample must be a **self-contained Stride Game Package, created with Game Studio**

	- This means that a sample package **must not reference** assets/files outside its directory.

* A sample package must use a package name that is unique and can be replaced by a simple regex
  in the files using it (.csproj, .cs, etc).

  For example: `SimpleAudio`.

* We are currently using the following directories as categories to group samples:

	- `Audio` : Samples related to audio.
	- `Games` : Small games.
	- `Graphics` : Graphics samples (display 3D models, sprites, text, etc).
	- `Input` : Input samples (touch, mouse, gamepad, etc).
	- `UI` : UI samples.

* `StrideSamples.sln` ia a top level solution referencing all Game Packages (`sdpkg`).

* Inside each category, we store a package in its own directory.
  For example,`SimpleAudio` in `Audio`:

	- `Audio`
		- `SimpleAudio`
			- `.sdtpl` : Directory containing icons/screenshots used to display the template in the UI.
			- `Assets` : Assets files (`.sd` files).
			- `Resources` : Resource files (`.jpg`, `.fbx`, etc).
			- `SimpleAudio.Game` : Common Game code.
			- `SimpleAudio.Windows` : Windows Desktop executable.
			- `SimpleAudio.sdpkg` : Package description.
			- `SimpleAudio.sdtpl` : Package Template description.
