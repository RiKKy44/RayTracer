# Ray Tracer Implementation in C#

This project is a C# port of the ray tracer described in Peter Shirley's "Ray Tracing in One Weekend". The primary objective was to adapt the original C++ codebase to modern standards, focusing on memory efficiency and multithreading.

## Implementation Details

Unlike the reference implementation, this project introduces several optimizations specific to the .NET runtime:

**Concurrency and Thread Safety**
The rendering pipeline was modified to utilize `Parallel.For` instead of a single-threaded loop, allowing the application to utilize all available CPU cores. To ensure thread safety during parallel execution, I replaced standard random number generation with `Random.Shared`, which eliminates race conditions without the overhead of manual locking.

**Memory Optimization**
Mathematical primitives such as vectors (`Vec3`) were implemented as `readonly struct`. This design choice prevents the C# compiler from creating defensive copies when passing arguments by reference (`in` parameters), reducing overhead during heavy mathematical operations.

**Architecture**
The codebase is structured into logical namespaces (`Core`, `Geometry`, `Materials`, `Rendering`) rather than a monolithic file structure. Additionally, the legacy PPM output format was replaced with the ImageSharp library, enabling direct export to standard formats like JPG and PNG.

## Usage

To build and run the project with default settings:

```bash
dotnet run -c Release
```

The application accepts command-line arguments to modify rendering parameters without recompilation. Available flags include width (`-w`), samples per pixel (`-s`), max recursion depth (`-d`), and output filename (`-f`).

Example of a high-quality render command:

```bash
dotnet run -c Release -- -w 1920 -s 500 -d 50 -f final_output.jpg
```
