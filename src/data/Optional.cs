using System;

namespace EconSim.data;

public class Optional<T>
{
    private T value;
    private bool isPresent = false;

    public Optional() {}
    
    public Optional(T value) {
        this.value = value;
        this.isPresent = true;
    }

    public bool IsPresent() {
        return this.isPresent;
    }

    public T Get() {
        if (!isPresent)
            throw new ArgumentException("Tried to access a non present Optional.");
        
        return value;
    }

    public void Set(T value) {
        if (value != null) {
            this.value = value;
            this.isPresent = true;
        }
    }

    public static Optional<T> Empty() {
        return new Optional<T>();
    }
}