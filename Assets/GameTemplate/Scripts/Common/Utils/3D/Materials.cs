/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class Materials {
	public static void changeSharedMaterial(Renderer _renderer, int index, Material material){
		Material[] materials = _renderer.sharedMaterials;
		materials[index] = material;
		_renderer.materials = materials;
	}

	public static void changeSharedMaterial(Renderer _renderer, Material[] _materials){
		Material[] materials = _renderer.sharedMaterials;
		materials = _materials;
		_renderer.materials = materials;
	}
}
