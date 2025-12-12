# Events

A lightweight event system used to decouple gameplay, UI, and management logic.

The goal of this folder is to avoid tight coupling between systems by using
explicit events instead of direct references.

---

## Purpose

- Allow systems to communicate without knowing about each other
- Keep gameplay logic independent from UI and managers
- Make flow easier to follow and reason about

---

## Usage Pattern

Events are:
- Defined in one place
- Subscribed to by interested systems
- Invoked by the system that owns the responsibility

Typical flow:
- Gameplay triggers an event
- UI / GameManager reacts to it

---

## Usage Example

### UI (publisher)

```csharp
public class WinView : MonoBehaviour
{
    [SerializeField] private AScriptableEvent nextLevelEvent;

    public void OnNextLevelButton()
    {
        nextLevelEvent.Raise();
    }
}
````

### GameManager (subscriber)

```csharp
public class GameManager : MonoBehaviour
{
    [SerializeField] private AScriptableEvent nextLevelEvent;

    private void OnEnable()
    {
        nextLevelEvent.OnRaised += OnNextLevelEvent;
    }

    private void OnDisable()
    {
        nextLevelEvent.OnRaised -= OnNextLevelEvent;
    }

    private void OnNextLevelEvent()
    {
        LoadNextLevel();
    }
}
```

## Design Notes

* Events are explicit and readable
* No global static event buses
* No reflection or magic strings
* Easy to trace in code reviews

---

## Summary

This folder provides a simple, clear way to handle cross-system communication
without overengineering, suitable for small projects and case studies.

---
