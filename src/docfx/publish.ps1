docfx docfx.json
Copy-Item -Path ".\_site\*" -Destination "..\..\docs\" -Recurse -Force
