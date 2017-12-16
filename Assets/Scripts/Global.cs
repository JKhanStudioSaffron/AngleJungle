﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global
{
	public static bool isDragging=false;
	public static bool isPaused=false;
	public static int antiCheater=3;

	// Layer strings
	public const string LAYER_POWER_GEM = "PowerGem";

	// Tag strings
	public const string TAG_MIRROR_COLLIDER = "MirrorCollider";
	public const string TAG_DRAGGABLE = "Draggable";
	public const string TAG_HAND_TUTORIAL = "HandClickTutorial";
	public const string TAG_PROTRACTOR = "Protractor";
	public const string TAG_MUSIC_MANAGER = "MusicManager";
	public const string TAG_GO_BUTTON = "GoButton";

	// Scene strings
	public const string SCENE_MAP = "StageNew";
	public const string SCENE_TREASURE = "Treasure";
}
