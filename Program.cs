
using System;
using System.Collections.Generic;


IUObject test = new UObject();
test.addProperty<IVelocity>(new Velocity());
IProperty velocity = test.getProperty<IVelocity>();
velocity.setValue<int[]>(new int[2]{1,2});


foreach(var tmp in test.getProperty<IVelocity>().getValue<int[]>()){
    Console.Write($"{tmp} ");
}

interface IUObject {
    T getProperty<T>() where T : IProperty;

    void setProperty<T>(T newProperty) where T : IProperty;

    void addProperty<T>(T newProperty) where T : IProperty;

    void dropProperty<T>() where T : IProperty;
}


//Непосредственно реализация универсального объекта
class UObject : IUObject{
    private Dictionary<Type, IProperty> _store = new();

    public T getProperty<T>() where T : IProperty{
        if(_store.ContainsKey(typeof(T))){
            return (T)_store[typeof(T)];
        }
        else{
            throw new PropertyException($"Отсутствует свойство {typeof(T)}");
        }
        
    }

    public void setProperty<T>(T newPropertyValue) where T : IProperty{
        if(_store.ContainsKey(typeof(T))){
            _store[typeof(T)] = newPropertyValue; 
        }
        else{
            throw new PropertyException($"Отсутствует свойство {typeof(T)}");
        }
    }

    public void addProperty<T>(T newProperty) where T : IProperty{
        if(!_store.ContainsKey(typeof(T))){
            _store.Add(typeof(T), newProperty);
        }
        else{
            throw new PropertyException($"Свойство {typeof(T)} уже существует");
        }
    }

    public void dropProperty<T>() where T : IProperty{
        if(!_store.Remove(typeof(T))){
            throw new PropertyException($"Свойства {typeof(T)} не существует");
        }
    }

}

class PropertyException : Exception{
    public PropertyException(string message) : base(message){}
}

interface IProperty{
    T getValue<T>();
    void setValue<T>(T newValue);
};

interface IVelocity : IProperty{
    //Методы для работы со скоростью
}

class Velocity : IProperty, IVelocity{
    private int[] _vel = new int[2];

    public T getValue<T>(){
        return (T)(object)_vel;
    }
    
    public void setValue<T>(T newValue){
        if(newValue != null){
            _vel = (int[])(object)newValue;
        }
        else{
            _vel = new int[2];
        }
    }
}