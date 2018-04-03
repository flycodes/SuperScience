﻿#if UNITY_EDITOR
using UnityEngine;

namespace Unity.Labs.SuperScience
{
    public class GizmoModule : MonoBehaviour
    {
        public static GizmoModule instance;

        public const float rayLength = 100f;
        const float k_RayWidth = 0.001f;

        [SerializeField]
        [Tooltip("Default sphere mesh used for drawing gizmo spheres.")]
        Mesh m_SphereMesh;

        [SerializeField]
        [Tooltip("Default cube mesh used for drawing gizmo boxes and rays.")]
        Mesh m_CubeMesh;

        MaterialPropertyBlock m_GizmoProperties;
        
        public Material gizmoMaterial
        {
            get { return m_GizmoMaterial; }
        }

        [SerializeField]
        Material m_GizmoMaterial;

        void Awake()
        {
            instance = this;
            m_GizmoProperties = new MaterialPropertyBlock();
        }
        
        /// <summary>
        /// Draws a ray for a single frame in all camera views
        /// </summary>
        /// <param name="origin">Where the ray should begin, in world space</param>
        /// <param name="direction">Which direction the ray should point, in world space</param>
        /// <param name="color">What color to draw the ray with</param>
        /// <param name="viewerScale">Optional global scale to apply to match a scaled user</param>
        /// <param name="rayLength">How long the ray should extend</param>
        public void DrawRay(Vector3 origin, Vector3 direction, Color color, float viewerScale = 1f, float rayLength = rayLength)
        {
            direction.Normalize();
            m_GizmoProperties.SetColor("_Color", color);
            
            var rayPosition = origin + direction * rayLength * 0.5f;
            var rayRotation = Quaternion.LookRotation(direction);
            var rayWidth = k_RayWidth * viewerScale;
            var rayScale = new Vector3(rayWidth, rayWidth, rayLength);
            var rayMatrix = Matrix4x4.TRS(rayPosition, rayRotation, rayScale);

            Graphics.DrawMesh(m_CubeMesh, rayMatrix, m_GizmoMaterial, 0, null, 0, m_GizmoProperties);
        }

        /// <summary>
        /// Draws a sphere for a single frame in all camera views
        /// </summary>
        /// <param name="center">The center of the sphere, in world space</param>
        /// <param name="radius">The radius of the sphere, in meters</param>
        /// <param name="color">What color to draw the sphere with</param>
        public void DrawSphere(Vector3 center, float radius, Color color)
        {
            m_GizmoProperties.SetColor("_Color", color);

            var sphereMatrix = Matrix4x4.TRS(center, Quaternion.identity, Vector3.one * radius);
            Graphics.DrawMesh(m_SphereMesh, sphereMatrix, m_GizmoMaterial, 0, null, 0, m_GizmoProperties);
        }

        /// <summary>
        /// Draws a cube for a single frame in all camera views
        /// </summary>
        /// <param name="position">The center of the cube, in world space</param>
        /// <param name="rotation">The orientation of the cube, in world space</param>
        /// <param name="scale">The scale of the cube</param>
        /// <param name="color">What color to draw the cube with</param>
        public void DrawCube(Vector3 position, Quaternion rotation, Vector3 scale, Color color)
        {
            m_GizmoProperties.SetColor("_Color", color);

            var cubeMatrix = Matrix4x4.TRS(position, rotation, scale);
            Graphics.DrawMesh(m_CubeMesh, cubeMatrix, m_GizmoMaterial, 0, null, 0, m_GizmoProperties);
        }
    }
}
#endif
