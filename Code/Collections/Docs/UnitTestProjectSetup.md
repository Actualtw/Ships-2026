# Godot 4.7 + C# Unit Test Project Setup

This guide shows how to add a separate unit test project to a Godot C# game.

Audience: students learning test-driven development and basic project structure.

## Goal

Create a dedicated .NET test project that can test game logic classes (collections, math, state logic, utility code) without launching Godot.

## Prerequisites

- Godot project using C# and .NET
- .NET SDK installed (project currently targets net8.0)
- Terminal opened at the project root

## Why a Separate Test Project?

- Keeps test code separate from gameplay/runtime code
- Makes tests fast to run with dotnet test
- Works well in CI/CD pipelines
- Avoids mixing test classes into the game assembly

## Step 1: Create a Test Project Folder

From the project root:

~~~bash
dotnet new xunit -n Ships.Tests
~~~

This creates:

- Ships.Tests/Ships.Tests.csproj
- starter test file(s)

## Step 2: Reference the Game Project

Edit Ships.Tests/Ships.Tests.csproj and add a project reference:

~~~xml
<ItemGroup>
  <ProjectReference Include="../Ships.csproj" />
</ItemGroup>
~~~

This lets tests use namespaces and classes from the main game project.

## Step 3: Exclude Test Sources from the Godot Project

Important with SDK-style projects: C# source files are included by default.
If test files live under the same repo, exclude them from Ships.csproj.

Add this to Ships.csproj:

~~~xml
<ItemGroup>
  <Compile Remove="Ships.Tests/**/*.cs" />
</ItemGroup>
~~~

Without this, test classes can accidentally compile into the game assembly.

## Step 4: Add Test Project to the Solution

Run:

~~~bash
dotnet sln Ships.sln add Ships.Tests/Ships.Tests.csproj
~~~

This ensures solution-wide build/test tools see the test project.

## Step 5: Write the First Unit Tests

Example structure:

- Ships.Tests/PriorityQueueTests.cs
- Ships.Tests/LinkedListTests.cs

A simple test pattern:

~~~csharp
using Xunit;

public class ExampleTests
{
 [Fact]
 public void MethodName_ExpectedBehavior()
 {
  // Arrange
  // Act
  // Assert
 }
}
~~~

Naming suggestion for beginners:

MethodName_Condition_ExpectedResult

## Step 6: Run the Tests

Run all tests in the test project:

~~~bash
dotnet test Ships.Tests/Ships.Tests.csproj
~~~

Or run all tests in the solution:

~~~bash
dotnet test Ships.sln
~~~

## Step 7: Typical Classroom Workflow

1. Implement a small feature in a logic class.
2. Add 1-3 tests for expected behavior and edge cases.
3. Run dotnet test.
4. Refactor while tests stay green.

## What to Unit Test in a Godot C# Game

Great candidates:

- Custom collections
- Pathfinding/algorithms
- Rules and validation logic
- Save/load data transformations
- Utility extensions

Avoid in pure unit tests:

- SceneTree lifecycle behavior
- Rendering/physics timing behavior
- Signal/frame-order interactions

Those are better as integration tests.

## Common Errors and Fixes

Error: ambiguous type names (for example LinkedList)

- Use the fully qualified type name in tests (for example GA.Collections.LinkedList<int>)

Error: test project cannot find game classes

- Verify ProjectReference exists in Ships.Tests.csproj

Error: tests compile into game project

- Verify Compile Remove="Ships.Tests/**/*.cs" exists in Ships.csproj

## Optional: Coverage Collection

The test project can include coverlet.collector.
Run with coverage output:

~~~bash
dotnet test Ships.Tests/Ships.Tests.csproj --collect:"XPlat Code Coverage"
~~~

## Minimal Checklist

- Ships.Tests project created
- ProjectReference to Ships.csproj
- Test sources excluded from Ships.csproj
- Test project added to Ships.sln
- At least one passing test file

---

This setup gives students a clean, industry-standard testing workflow while keeping Godot runtime concerns separate from logic testing.
