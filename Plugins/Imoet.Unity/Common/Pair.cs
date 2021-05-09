using System;

[Serializable]
class Pair<key, value> {
    public key Key;
    public value Value;
    public Pair(key Key, value Value) {
        this.Key = Key;
        this.Value = Value;
    }
}