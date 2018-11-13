///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/15
//用途：蒙皮工具类
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public static class MeshHelper {

    public class SkinnedMeshInfo {
        public Mesh mesh = null;
        public Material[] materials;
        public Transform[] bones;
    }

    public class MeshInfo {
        public Mesh mesh = null;
        public Material[] materials;
    }


    /// <summary>
    /// 骨骼
    /// </summary>
    /// <param name="_root"></param>
    /// <returns></returns>
    public static Dictionary<string, Transform> CacheBones ( Transform _root ) {
        Dictionary<string, Transform> cachedBones = new Dictionary<string, Transform>();

        Transform[] transforms = _root.GetComponentsInChildren<Transform>();
        foreach (Transform t in transforms)
        {
			cachedBones[t.name] = t;
        }
        return cachedBones;
    }


    public static Transform GetBone(Transform _obj, string _boneName)
    {
        if (_boneName == string.Empty) return null;
        if (_obj.name == _boneName) return _obj;
        for (int i = 0; i < _obj.transform.childCount; i++)
        {
            Transform bone = GetBone(_obj.transform.GetChild(i), _boneName);
            if (bone != null) return bone;
        }
        return null;
    }

    #region SkinnedMeshRenderer 的模型合并并蒙皮
    /// <summary>
    /// 我们只要smrList中只有一种Material
    /// </summary>
    class SkinnedMeshCombineInfo {
        public Material material = null;
        public List<SkinnedMeshRenderer> smrList = new List<SkinnedMeshRenderer>();
    }
    // ------------------------------------------------------------------ 

    public static SkinnedMeshInfo CombineSkinnedMeshRenderer ( List<SkinnedMeshRenderer> _smrList, 
                                                               Dictionary<string, Transform> _refBones ) 
    {
        List<SkinnedMeshCombineInfo> skinnedMeshCombineInfoList = new List<SkinnedMeshCombineInfo>();

        //根据材质对蒙皮信息进行整理，加入到skinnedMeshCombineInfoList队列中去
        for (int y = 0; y < _smrList.Count; y++)
        {
            SkinnedMeshRenderer smr = _smrList[y];
            for (int i = 0; i <  smr.sharedMaterials.Length; i++)
            {
                Material material = smr.sharedMaterials[i];
                if (material == null)
                {
                    continue;
                }
                SkinnedMeshCombineInfo smci = null;
                for (int x = 0; x < skinnedMeshCombineInfoList.Count; ++x)
                {
                    if (ReferenceEquals(skinnedMeshCombineInfoList[i].material, material))
                    {
                        smci = skinnedMeshCombineInfoList[i];
                        break;
                    }
                }
                if (smci == null)
                {
                    smci = new SkinnedMeshCombineInfo();
                    smci.material = material;
                    skinnedMeshCombineInfoList.Add(smci);
                }
                smci.smrList.Add(smr);
            }
        }

        //合并列表
        List<CombineInstance> ciList = new List<CombineInstance>();
        //骨骼列表
        List<Transform> bones = new List<Transform>();
        //材质列表
        List<Material> materials = new List<Material>();

        //遍历skinnedMeshCombineInfoList队列
        for (int i = 0; i < skinnedMeshCombineInfoList.Count; i++)
        {
            SkinnedMeshCombineInfo smrCombineInfo = skinnedMeshCombineInfoList[i];

            //对同一种材质的mesh进行合并后加入到合并列表中去
            Mesh combinedMesh = CombineSkinnedMeshWithSameMaterial (smrCombineInfo.smrList);
            CombineInstance ci = new CombineInstance();
            ci.mesh = combinedMesh;
            ciList.Add (ci);

            //获取骨骼加入到骨骼列表中去
            for (int j = 0; j < smrCombineInfo.smrList.Count; j++)
            {
                SkinnedMeshRenderer smr = smrCombineInfo.smrList[j];
                for (int x = 0; x < smr.bones.Length; x++)
                {
                    Transform bone = smr.bones[x];
                    if (!_refBones.ContainsKey(bone.name) || _refBones[bone.name] == null)
                    {
                        Debug.LogError ( "Failed to combine skinned mesh renderer, can't find the bone " + bone.name );
                        return null;
                    }
                    bones.Add(_refBones[bone.name]);
                }
            }

            //加入到材质列表中去（之前的处理中已经确认了其中的是单例材质）
            materials.Add(smrCombineInfo.material);
        }
        //最终处理
        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes ( ciList.ToArray(), false, false );

        //Combine以后删除到这些合并列表源，他们再也用不到了。
        for (int i = 0; i < ciList.Count; i++)
        {
             Object.DestroyImmediate(ciList[i].mesh);
        }

        //
        SkinnedMeshInfo skinnedMeshInfo = new SkinnedMeshInfo();
        skinnedMeshInfo.mesh = finalMesh;
        skinnedMeshInfo.materials = materials.ToArray();
        skinnedMeshInfo.bones = bones.ToArray();
        return skinnedMeshInfo;
    }


    /// <summary>
    /// 对使用同一种材质的SkinnedMeshRenderer进行mesh合并
    /// </summary>
    /// <param name="_smrList"></param>
    /// <returns></returns>
    public static Mesh CombineSkinnedMeshWithSameMaterial(List<SkinnedMeshRenderer> _smrList)
    {
        List<CombineInstance> ciList = new List<CombineInstance>();
        for (int i = 0; i < _smrList.Count; ++i)
        {
            CombineInstance ci = new CombineInstance();
            ci.mesh = _smrList[i].sharedMesh;
			if(ci.mesh != null){//
            	ciList.Add(ci);
			}
        }
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(ciList.ToArray(), true, false);
        return mesh;
    }
    #endregion




    #region 对MeshFilter进行合并
    /// <summary>
    /// 我们只要smrList中只有一种Material
    /// </summary>
    class MeshCombineInfo {
        public Material material = null;
        public List<MeshFilter> meshFilterList = new List<MeshFilter>();
    }


	[System.Obsolete("Use another overload method instead")]
	public static MeshInfo CombineMeshFilters(MeshFilter[] _meshFilterList) {
		return CombineMeshFilters (null, _meshFilterList);
	}
    public static MeshInfo CombineMeshFilters ( Transform root, MeshFilter[] _meshFilterList ) {
        List<MeshCombineInfo> meshCombineInfoList = new List<MeshCombineInfo>();

        //根据材质将源数据整理到meshCombineInfoList列表中去
        foreach ( MeshFilter meshFilter in _meshFilterList ) {
            if ( meshFilter.GetComponent<Renderer>() == null )
                continue;

            int idx = -1;
            for ( int i = 0; i < meshCombineInfoList.Count; ++i ) {
                if ( meshCombineInfoList[i].material == meshFilter.GetComponent<Renderer>().sharedMaterial ) {
                    idx = i;
                    break;
                }
            }
            if ( idx == -1 ) {
                MeshCombineInfo info = new MeshCombineInfo();
                info.material = meshFilter.GetComponent<Renderer>().sharedMaterial;
                info.meshFilterList.Add(meshFilter);
                meshCombineInfoList.Add (info);
            }
            else {
                meshCombineInfoList[idx].meshFilterList.Add(meshFilter);
            }
        }

        //合并列表
        List<CombineInstance> ciList = new List<CombineInstance>();
        //材质列表
        List<Material> materials = new List<Material>();    


        foreach ( MeshCombineInfo mrCombineInfo in meshCombineInfoList ) {

            //先将同材质的合并，将合并后的加入到合并列表中去
            Mesh combinedMesh = CombineMeshWithSameMaterial(root, mrCombineInfo.meshFilterList);
            CombineInstance ci = new CombineInstance();
            ci.mesh = combinedMesh;
            ciList.Add (ci);

            //将单例材质加入到材质列表中
            materials.Add(mrCombineInfo.material);
        }

        //最终处理
        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes ( ciList.ToArray(), false, false );

        //将再也用不到的合并前的源数据删除，因为再也用不到他们了
        foreach ( CombineInstance ci in ciList ) {
            Object.DestroyImmediate(ci.mesh);
        }


        MeshInfo meshInfo = new MeshInfo();
        meshInfo.mesh = finalMesh;
        meshInfo.materials = materials.ToArray();
        return meshInfo;
    }




    [System.Obsolete("Use another overload method instead")]
	public static Mesh CombineMeshWithSameMaterial(List<MeshFilter> _meshFilterList) {
		return CombineMeshWithSameMaterial(null, _meshFilterList);
	}

    public static Mesh CombineMeshWithSameMaterial ( Transform root, List<MeshFilter> _meshFilterList ) {
		Quaternion originalR;
		Vector3 originalS;
		Vector3 originalT;
		if (root != null) {
			///使根节点的localToWorldMatrix为identity
			originalR = root.localRotation;
			originalS = root.localScale;
			originalT = root.localPosition;
			root.position = Vector3.zero;
			root.rotation = Quaternion.identity;
			Vector3 worldScale = root.lossyScale;
			root.localScale = new Vector3(1.0f / worldScale.x, 1.0f / worldScale.y, 1.0f / worldScale.z);
		}
		else {
			originalR = Quaternion.identity;
			originalS = Vector3.zero;
			originalT = Vector3.zero;
		}
	
		///combine
        List<CombineInstance> ciList = new List<CombineInstance>();
        foreach ( MeshFilter meshFilter in _meshFilterList ) {
            CombineInstance ci = new CombineInstance();
            ci.mesh = meshFilter.sharedMesh;
            ci.transform = meshFilter.transform.localToWorldMatrix;
            ciList.Add(ci);
        }
        Mesh newMesh = new Mesh();
        newMesh.CombineMeshes(ciList.ToArray(), true, true);

		if (root != null) {
			///还原根节点transform
			root.localPosition = originalT;
			root.localScale = originalS;
			root.localRotation = originalR;
		}
        return newMesh;
    }
    #endregion
}
