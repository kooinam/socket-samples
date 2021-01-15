using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;

public class DateTimeConverter : fsDirectConverter
{
    public override System.Type ModelType { get { return typeof(DateTime); } }

    public override object CreateInstance(fsData data, System.Type storageType)
    {
        return new DateTime();
    }

    public DateTime UnixTimeToDateTime(long unixtime)
    {
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixtime).ToLocalTime();

        return dtDateTime;
    }

    public override fsResult TrySerialize(object instance, out fsData serialized, System.Type storageType)
    {
        serialized = new fsData(0);

        return fsResult.Success;
    }

    public override fsResult TryDeserialize(fsData data, ref object instance, System.Type storageType)
    {
        int x = (int)data.AsInt64;

        instance = this.UnixTimeToDateTime(x);

        return fsResult.Success;
    }
}

public class Vector2IntConverter : fsDirectConverter
{
    public override System.Type ModelType { get { return typeof(Vector2Int); } }

    public override object CreateInstance(fsData data, System.Type storageType)
    {
        return new Vector2Int();
    }

    public override fsResult TrySerialize(object instance, out fsData serialized, System.Type storageType)
    {
        serialized = new fsData(((Vector2Int)instance).x);

        return fsResult.Success;
    }

    public override fsResult TryDeserialize(fsData data, ref object instance, System.Type storageType)
    {
        int x = (int)data.AsDictionary["x"].AsInt64;
        int y = (int)data.AsDictionary["y"].AsInt64;

        instance = new Vector2Int(x, y);

        return fsResult.Success;
    }
}

public class ListConverter : fsDirectConverter
{
    public override System.Type ModelType { get { return typeof(List<Dictionary<string, object>>); } }

    public override object CreateInstance(fsData data, System.Type storageType)
    {
        return new List<Dictionary<string, object>>();
    }

    public override fsResult TrySerialize(object instance, out fsData serialized, System.Type storageType)
    {
        List<Dictionary<string, object>> dicts = instance as List<Dictionary<string, object>>;
        List<fsData> datas = new List<fsData>();

        foreach (Dictionary<string, object> dict in dicts)
        {
            Dictionary<string, fsData> data = new Dictionary<string, fsData>();

            foreach (string key in dict.Keys)
            {
                if (dict[key] is string)
                {
                    data[key] = new fsData(dict[key] as string);
                }

                if (dict[key] is int)
                {
                    data[key] = new fsData((int)dict[key]);
                }
            }

            datas.Add(new fsData(data));
        }

        serialized = new fsData(datas);

        return fsResult.Success;
    }

    public override fsResult TryDeserialize(fsData data, ref object instance, System.Type storageType)
    {
        // int x = (int)data.AsDictionary["x"].AsInt64;
        // int y = (int)data.AsDictionary["y"].AsInt64;

        // instance = new Vector2Int(x, y);

        return fsResult.Success;
    }
}

public class DictionaryConverter : fsDirectConverter
{
    public override System.Type ModelType { get { return typeof(Dictionary<string, object>); } }

    public override object CreateInstance(fsData data, System.Type storageType)
    {
        return new Dictionary<string, object>();
    }

    private fsData toFSData(object v)
    {
        fsData d = null;

        if (v is string)
        {
            d = new fsData(v as string);
        }

        if (v is int)
        {
            d = new fsData((int)v);
        }

        if (v is long)
        {
            d = new fsData((long)v);
        }

        if (v is double)
        {
            d = new fsData((double)v);
        }

        if (v is List<Dictionary<string, object>>)
        {
            List<Dictionary<string, object>> dicts = v as List<Dictionary<string, object>>;
            List<fsData> datas = new List<fsData>();

            foreach (Dictionary<string, object> dict in dicts)
            {
                Dictionary<string, fsData> data = new Dictionary<string, fsData>();

                foreach (string key in dict.Keys)
                {
                    data[key] = this.toFSData(dict[key]);
                }

                datas.Add(new fsData(data));
            }

            d = new fsData(datas);
        }

        if (v is Dictionary<string, object>) {
            Dictionary<string, fsData> data = new Dictionary<string, fsData>();

            foreach (string key in (v as Dictionary<string, object>).Keys)
            {
                data[key] = this.toFSData((v as Dictionary<string, object>)[key]);
            }

            d = new fsData(data);
        }

        return d;
    }

    public override fsResult TrySerialize(object instance, out fsData serialized, System.Type storageType)
    {
        Dictionary<string, object> dict = instance as Dictionary<string, object>;
        Dictionary<string, fsData> data = new Dictionary<string, fsData>();

        foreach (string key in dict.Keys)
        {
            fsData d = this.toFSData(dict[key]);

            if (d != null)
            {
                data[key] = d;
            }
        }

        serialized = new fsData(data);

        return fsResult.Success;
    }

    public override fsResult TryDeserialize(fsData data, ref object instance, System.Type storageType)
    {
        Dictionary<string, object> newInstance = new Dictionary<string, object>();
        Dictionary<string, fsData> dict = data.AsDictionary;

        foreach (KeyValuePair<string, fsData> kvp in dict)
        {
            if (kvp.Value.Type == fsDataType.Int64)
            {
                newInstance[kvp.Key] = kvp.Value.AsInt64;
            }

            if (kvp.Value.Type == fsDataType.Double)
            {
                newInstance[kvp.Key] = kvp.Value.AsDouble;
            }
        }

        instance = newInstance;

        // int x = (int)data.AsDictionary["x"].AsInt64;
        // int y = (int)data.AsDictionary["y"].AsInt64;

        // instance = new Vector2Int(x, y);

        return fsResult.Success;
    }
}

public class DictionarySerializer
{
    public static string ToJSON(Dictionary<string, object> dict)
    {
        fsSerializer serializer = new fsSerializer();
        serializer.AddConverter(new DictionaryConverter());

        fsData data;
        serializer.TrySerialize<Dictionary<string, object>>(dict, out data).AssertSuccessWithoutWarnings();

        return fsJsonPrinter.CompressedJson(data);
    }
}

public class Serializer
{
    public static T FromJSON<T>(string payload) where T : new()
    {
        fsSerializer serializer = new fsSerializer();
        serializer.AddConverter(new DateTimeConverter());

        fsData data = fsJsonParser.Parse(payload);
        T item = new T();

        JSONKey jsonKey = (JSONKey)System.Attribute.GetCustomAttribute(typeof(T), typeof(JSONKey));
        string key = jsonKey.Single;

        if (data.AsDictionary.ContainsKey(key))
        {
            fsData itemData = data.AsDictionary[key];

            serializer.TryDeserialize<T>(itemData, ref item).AssertSuccessWithoutWarnings();
        }

        return item;
    }
}

public class JSONKey : System.Attribute
{
    public string Single { get; set; }
    public string List { get; set; }
}
