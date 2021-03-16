using System.Collections.Generic;
public class BiMap<T1, T2>
{
    private Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
    private Dictionary<T2, T1> _reverse = new Dictionary<T2, T1>();

    public BiMap()
    {
        this.Forward = new Indexer<T1, T2>(_forward);
        this.Reverse = new Indexer<T2, T1>(_reverse);
    }

    public class Indexer<T3, T4>
    {
        private Dictionary<T3, T4> _dictionary;
        public Indexer(Dictionary<T3, T4> dictionary)
        {
            _dictionary = dictionary;
        }
        public T4 this[T3 index]
        {
            get { return _dictionary[index]; }
            set { _dictionary[index] = value; }
        }
    }

    public bool Contains(T1 key) => _forward.ContainsKey(key);
    public bool Contains(T2 key) => _reverse.ContainsKey(key);

    public void Add(T1 t1, T2 t2)
    {
        _forward.Add(t1, t2);
        _reverse.Add(t2, t1);
    }

    public void Remove(T1 key)
    {
        if (!Contains(key))
            return;
        _reverse.Remove(_forward[key]);
        _forward.Remove(key);
    }

    public void Remove(T2 key)
    {
        if (!Contains(key))
            return;
        Remove(_reverse[key]);
    }

    public Dictionary<T1, T2>.KeyCollection GetKeys() => _forward.Keys;

    public T2 GetOrDefault(T1 key, T2 defaultVal) => 
        _forward.ContainsKey(key) ? _forward[key] : defaultVal;
    public T1 GetOrDefault(T2 key, T1 defaultVal) =>
        _reverse.ContainsKey(key) ? _reverse[key] : defaultVal;

    public Indexer<T1, T2> Forward { get; private set; }
    public Indexer<T2, T1> Reverse { get; private set; }
}