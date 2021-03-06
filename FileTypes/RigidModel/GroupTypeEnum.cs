﻿using System;

namespace Filetypes.RigidModel
{
    public enum GroupTypeEnum
    {
		bow_wave = 22,
		non_renderable = 26,
		texture_combo_vertex_wind = 29,
		texture_combo = 30,
		decal_waterfall = 31,
		standard_simple = 32,
		campaign_trees = 34,
		point_light = 38,
		static_point_light = 45,
		debug_geometry = 46,
		custom_terrain = 49,
		weighted_cloth = 58,
		cloth = 60,
		collision = 61,
		collision_shape = 62,
		tiled_dirtmap = 63,
		ship_ambientmap = 64,
		weighted = 65,
		projected_decal = 67,
		default_type = 68,
		grass = 69,
		weighted_skin = 70,
		decal = 71,
		decal_dirtmap = 72,
		dirtmap = 73,
		tree = 74,
		tree_leaf = 75,
		weighted_decal = 77,
		weighted_decal_dirtmap = 78,
		weighted_dirtmap = 79,
		weighted_skin_decal = 80,
		weighted_skin_decal_dirtmap = 81,
		weighted_skin_dirtmap = 82,
		water = 83,
		unlit = 84,
		weighted_unlit = 85,
		terrain_blend = 86,
		projected_decal_v2 = 87,
		ignore = 88,
		tree_billboard_material = 89,
		water_displace_volume = 91,
		rope = 93,
		campaign_vegetation = 94,
		projected_decal_v3 = 95,
		weighted_texture_blend = 96,
		projected_decal_v4 = 97,
		global_terrain = 98,
		decal_overlay = 99,
		alpha_blend = 100
	};

	public enum AlphaMode : Int32
	{
		Opaque = 0,
		Alpha_Test = 1,
		Alpha_Blend = -1
	};

	public enum VertexFormat : UInt32
	{
		Unknown = 99,
		Default = 0,
		Weighted = 3,
		Cinematic = 4
	};

	/*
	enum class RigidMaterial : uint32_t
	{
		bow_wave					= 22,
	non_renderable				= 26,
	texture_combo_vertex_wind	= 29,
	texture_combo				= 30,
	decal_waterfall				= 31,
	standard_simple				= 32,
	campaign_trees				= 34,
	point_light					= 38,
	static_point_light			= 45,
	debug_geometry				= 46,
	custom_terrain				= 49,
	weighted_cloth				= 58,
	cloth						= 60,
	collision					= 61,
	collision_shape				= 62,
	tiled_dirtmap				= 63,
	ship_ambientmap				= 64,
	weighted					= 65,
	projected_decal				= 67,
	default						= 68,
	grass						= 69,
	weighted_skin				= 70,
	decal						= 71,
	decal_dirtmap				= 72,
	dirtmap						= 73,
	tree						= 74,
	tree_leaf					= 75,
	weighted_decal				= 77,
	weighted_decal_dirtmap		= 78,
	weighted_dirtmap			= 79,
	weighted_skin_decal			= 80,
	weighted_skin_decal_dirtmap	= 81,
	weighted_skin_dirtmap		= 82,
	water						= 83,
	unlit						= 84,
	weighted_unlit				= 85,
	terrain_blend				= 86,
	projected_decal_v2			= 87,
	ignore						= 88,
	tree_billboard_material		= 89,
	water_displace_volume		= 91,
	rope						= 93,
	campaign_vegetation			= 94,
	projected_decal_v3			= 95,
	weighted_texture_blend		= 96,
	projected_decal_v4			= 97,
	global_terrain				= 98,
	decal_overlay				= 99,
	alpha_blend					= 100
};
	*/

}
