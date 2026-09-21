**Area costs**

Navigable regions
Calm 1
Shallow 2
Swamp 3
Strong current 4
Rocky 5
Very strong current 6
Storm 7

Non-navigable regions
Islands -1
Solid obstacles -1 (Rocks, shipwrecks, reefs etc.)
Bounds -1 (World boundary)

**Detecting different water types**

Area3D for navigable and non-navigable regions.
Each region is configured to be either navigable or non-navigable.
Alternatively regions can also contain their specific type.
Navigable regions have navigation costs configured.
Each Area3D needs a CollisionShape3D so that NavigationGrid can detect it.

When building the grid each cell performs a physics query at its position to check where it is located.
If an obstacle is found the navigation cost is set to -1.
If a navigable region is found the navigation cost is set to the configured cost of the area.
Otherwise return default navigation cost (1).
