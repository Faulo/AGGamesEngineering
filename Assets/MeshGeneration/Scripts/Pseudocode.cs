using UnityEngine;

namespace AGGE.MeshGeneration {
    public static class PseudoCode {
        // Pseudoimplementierung - nicht die tatsächliche API
        public struct PointMesh {
            Vertex[] vertices;
            Vector1Int[] points;
        }
        public struct LineMesh {
            Vertex[] vertices;
            Vector2Int[] lines;
        }
        public struct TriangleMesh {
            Vertex[] vertices;
            Vector3Int[] triangles;
        }
        public struct QuadMesh {
            Vertex[] vertices;
            Vector4Int[] quads;
        }

        // Pseudoimplementierung - nicht die tatsächliche API
        public struct Vertex {
            public Vector3 position;
            public Vector3 normal;
            public Vector3 tangent;
        }

        public struct Vector1Int {
        }
        public struct Vector4Int {
        }

        public class Mesh {
            Vector3[] vertices;
            Vector3[] normals;
            Vector3[] tangents;
            int[] triangles;

            public void RecalculateNormals() { }
            public void RecalculateTangents() { }
        }
    }
}