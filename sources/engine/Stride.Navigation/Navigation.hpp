// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#pragma once
#pragma pack(push, 4)
struct Point
{
	int X;
	int Y;
};
struct BuildSettings
{
	// Bounding box for the generated navigation mesh
	BoundingBox boundingBox;
	float cellHeight;
	float cellSize;
	int tileSize;
	Point tilePosition;
	int regionMinArea;
	int regionMergeArea;
	float edgeMaxLen;
	float edgeMaxError;
	float detailSampleDistInput;
	float detailSampleMaxErrorInput;
	float agentHeight;
	float agentRadius;
	float agentMaxClimb;
	float agentMaxSlope;
};
struct GeneratedData
{
	bool success;
	Vector3* navmeshVertices = nullptr;
	int numNavmeshVertices = 0;
	uint8_t* navmeshData = nullptr;
	int navmeshDataLength = 0;
};
#pragma pack(pop)
