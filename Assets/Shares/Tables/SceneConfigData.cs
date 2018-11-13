using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class SceneConfigData : ScriptableObject
{
    public int instanceID;
    public string Name;
    public List<string> materialDataList = new List<string>();
    public List<string> shaderDataList = new List<string>();
    public List<string> textureDataList = new List<string>();

    public SceneAsset sceneAsset;
    public List<MaterialAsset> materialAssetList = new List<MaterialAsset>();



}

[System.Serializable]
public class SceneAsset
{
    public string sceneName;
    public GameObjectRelationship gameObjectRelationship;

    public SceneAsset(GameObject _sceneObj)
    {
        sceneName = _sceneObj.name;
        gameObjectRelationship = BuildGameObjRelationFor465(_sceneObj as GameObject);
    }
    /// <summary>
    /// 针对unity4.65版本以后的纪录方式,避免序列化对象中的循环嵌套 by吴江
    /// </summary>
    /// <param name="_obj"></param>
    /// <returns></returns>
    GameObjectRelationship BuildGameObjRelationFor465(GameObject _obj)
    {
        GameObjectRelationship data = new GameObjectRelationship();
        if (_obj == null) return data;
        GetRelationShipStructList(data.relationList, _obj, 0);
        return data;
    }

    void GetRelationShipStructList(List<RelationShipStruct> _already, GameObject _obj, int _level)
    {
        if (_obj == null) return;
        RelationShipStruct data = new RelationShipStruct();
        data.objName = _obj.name;
        data.instanceID = _obj.GetInstanceID();
        if (_obj.transform.parent != null)
        {
            data.parentName = _obj.transform.parent.gameObject.name;
            data.parentInstanceID = _obj.transform.parent.gameObject.GetInstanceID();
        }
        data.level = _level;
        Renderer rd = _obj.GetComponent<Renderer>();
        if (rd == null) rd = _obj.GetComponent<SkinnedMeshRenderer>();
        if (rd != null)
        {
            foreach (var item in rd.sharedMaterials)
            {
                if (item != null) data.matNames.Add(item.name.ToLower());
            }
        }
        _already.Add(data);
        int count = _obj.transform.childCount;
        _level++;
        for (int i = 0; i < count; i++)
        {
            GetRelationShipStructList(_already, _obj.transform.GetChild(i).gameObject, _level);
        }
    }



    GameObjectRelationship BuildGameObjRelation(GameObject _obj)
    {
        GameObjectRelationship data = new GameObjectRelationship();
        if (_obj == null) return data;
        data.objName = _obj.name;
        Renderer rd = _obj.GetComponent<Renderer>();
        if (rd == null) rd = _obj.GetComponent<SkinnedMeshRenderer>();
        if (rd != null)
        {
            foreach (var item in rd.sharedMaterials)
            {
                if (item != null) data.matNames.Add(item.name.ToLower());
            }
        }
        int count = _obj.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            data.childRelationList.Add(BuildGameObjRelation(_obj.transform.GetChild(i).gameObject));
        }
        return data;
    }
}

[System.Serializable]
public class MaterialAsset
{
    public string matName;
    public MaterialRelationship materialRelationship;

    public MaterialAsset(string _name, MaterialRelationship _materialRelationship)
    {
        matName = _name;
        materialRelationship = _materialRelationship;
    }

}



[System.Serializable]
public class GameObjectRelationship
{
    public int maxLevel;
    public string parentName = string.Empty;
    public string objName;
    public int instanceID;
    public int parentInstanceID;
    public List<string> matNames = new List<string>();
    public List<RelationShipStruct> relationList = new List<RelationShipStruct>();
    [System.NonSerialized]
    public List<GameObjectRelationship> childRelationList = new List<GameObjectRelationship>();


    /// <summary>
    /// 初始化数据,在经过这一步之前,数据禁止使用 by吴江
    /// </summary>
    public void InitData()
    {
        RelationShipStruct myRelationShipStruct = null;
        foreach (var item in relationList)
        {
            if (item.level == 0)
            {
                myRelationShipStruct = item;
                break;
            }
        }
        if (myRelationShipStruct != null)
        {
            objName = myRelationShipStruct.objName;
            matNames = myRelationShipStruct.matNames;
            instanceID = myRelationShipStruct.instanceID;
            parentInstanceID = myRelationShipStruct.parentInstanceID;
            relationList.Remove(myRelationShipStruct);
            childRelationList = InitSubData(relationList, 1, this);
        }
    }

    protected List<GameObjectRelationship> InitSubData(List<RelationShipStruct> _relationList, int _myLevel, GameObjectRelationship _parent)
    {
        //if (_myLevel > maxLevel) return new List<GameObjectRelationship>();
        List<GameObjectRelationship> resultList = new List<GameObjectRelationship>();
        foreach (RelationShipStruct item in _relationList)
        {
            if (item.level == _myLevel && item.parentName == _parent.objName && item.parentInstanceID == _parent.instanceID)
            {
                GameObjectRelationship gameObjectRelationship = new GameObjectRelationship();
                gameObjectRelationship.parentName = item.parentName;
                gameObjectRelationship.objName = item.objName;
                gameObjectRelationship.matNames = item.matNames;
                gameObjectRelationship.instanceID = item.instanceID;
                gameObjectRelationship.parentInstanceID = item.parentInstanceID;
                resultList.Add(gameObjectRelationship);
            }
        }
        _myLevel += 1;
        foreach (var item in resultList)
        {
            item.childRelationList = InitSubData(_relationList, _myLevel, item);
        }
        return resultList;
    }
}

[System.Serializable]
public class RelationShipStruct
{
    public int level;
    public string parentName;
    public string objName;
    public int instanceID;
    public int parentInstanceID;
    public List<string> matNames = new List<string>();
}



[System.Serializable]
public class MaterialRelationship
{
    public string matName;
    public string sdName;
    public List<PropertyTexturePair> propertyPairList = new List<PropertyTexturePair>();
}


[System.Serializable]
public class PropertyTexturePair
{
    public int type;
    public string propertyName;
    public string texName;
    public float value;
    public Color color;
    public Vector4 v4;

}