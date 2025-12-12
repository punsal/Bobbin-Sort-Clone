# Core Observables

This directory contains a lightweight, generic implementation of the Observer pattern. It provides a standardized way to create reactive data types that notify subscribers when their values change.

## Structure

*   **`Interface/IObservable<T>`**: Defines the contract for observable values, including the value property, modification method, and the change event.
*   **`Abstract/ObservableBase<T>`**: A base class that handles the underlying event subscription logic and resource disposal.
*   **`Observable<T>`**: The concrete implementation used to instantiate observable values.

## Usage Example

```csharp
// 1. Create an observable with an initial value
var health = new Observable<int>(100);

// 2. Subscribe to changes
health.OnChanged += (newValue) => 
{
    Debug.Log($"Health changed to: {newValue}");
};

// 3. Update value (triggers notification)
health.Set(90);

// 4. Update value silently (no notification)
health.Set(100, notify: false);

// 5. Clean up listeners
health.Dispose();