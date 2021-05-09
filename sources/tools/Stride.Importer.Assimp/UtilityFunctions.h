// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#include "Stdafx.h"

using namespace System;
using namespace Stride::Animations;
using namespace Stride::Core::Diagnostics;
using namespace Stride::Core::Mathematics;

// Assimp types convertion
String^ aiStringToString(aiString str);
Color aiColor4ToColor(aiColor4D color);
Color3 aiColor3ToColor3(aiColor3D color);
Color4 aiColor3ToColor4(aiColor3D color);
Matrix aiMatrixToMatrix(aiMatrix4x4 mat);
Vector2 aiVector2ToVector2(aiVector2D vec);
Vector3 aiVector3ToVector3(aiVector3D vec);
Quaternion aiQuaternionToQuaternion(aiQuaterniont<float> quat);
CompressedTimeSpan aiTimeToXkTimeSpan(double time, double tickPerSecond);

// Others
Vector3 QuaternionToEulerAngles(Quaternion q);
Vector3 FlipYZAxis(Vector3 input, bool shouldFlip);
