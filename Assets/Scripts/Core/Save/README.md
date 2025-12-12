# SaveManager

A minimal, extensible save system built on Unity `PlayerPrefs`.

It separates **save logic** from **save data**, allowing new fields to be added without modifying the save manager.

---

## Components

### SaveData

Base class for all save data.

```csharp
public abstract class SaveData
{
    public abstract string Key { get; }
}
````

Each derived save data class:

* Defines its own storage key
* Defines its own serializable fields

---

### SaveManager

Static utility responsible for saving and loading `SaveData`.

```csharp
public static void Save(SaveData data);
public static T Load<T>() where T : SaveData, new();
public static void Clear<T>() where T : SaveData, new();
```

* Uses JSON serialization
* Does not know or care about gameplay fields
* Keys are provided by `SaveData`

---

## Usage

```csharp
var data = SaveManager.Load<MySaveData>();
data.someValue = 10;
SaveManager.Save(data);
```

---

## Notes

* Built on `PlayerPrefs`
* Type-safe loading
* Easy to extend by adding new fields to `SaveData`
* Designed for small projects and case studies