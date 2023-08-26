# MeddyExplorer

GUI application utilizing Meddydata to find exactly what you want with ease!

## Build And Run

### Step 1: Open The MeddyDAM Solution And Run MeddyDAMMAUI

Get the build error "You must install NPM to build this project".

### Step 2: Download And Install Node.js With NPM And Try Again

Download LTS version of Node.js from https://nodejs.org/.

Run installer with "Automatically install the necessary tools. Note that this will also install Chocolatey. The script will pop-up in a new window after the installation completes." as true.

Open the MeddyDAM solution and now try running MeddyDAMMAUI.

Get the error about "npm -v" not working. But if you try it outside of Visual Studio, it does work.

Close Visual Studio and reopen the solution.

Try running MeddyDAMMAUI again.

Get an ERR_MODULE_NOT_FOUND error.

### Step 3: Delete "node_modules" Directory And Try Again

Delete the directory "MeddyDAM/MeddyDAMMAUI/node_modules".

Try running MeddyDAMMAUI again.

Builds successfully and runs!

But styles don't look tailwindcss-y, e.g., the headers have their own, unique styling.

Re-run the project.

The styles look correct now!
