using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace AGGE.ComputeShader {
    [ExecuteAlways]
    public class RayTraceComponent : MonoBehaviour {

        [SerializeField]
        Camera sourceCamera;

        [SerializeField]
        RawImage targetImage;

        [SerializeField]
        UnityEngine.ComputeShader raytracer;

        RenderTexture texture;

        protected void OnEnable() {
            RenderPipelineManager.beginCameraRendering += BeginCamera;
            RenderPipelineManager.endCameraRendering += EndCamera;
        }
        protected void OnDisable() {
            RenderPipelineManager.beginCameraRendering -= BeginCamera;
            RenderPipelineManager.endCameraRendering -= EndCamera;
        }

        void BeginCamera(ScriptableRenderContext context, Camera camera) {
            InitTexture();
        }
        void EndCamera(ScriptableRenderContext context, Camera camera) {
            SetShaderParameters();
            Render();
        }

        protected void OnDestroy() {
            DisposeTexture();
        }

        void DisposeTexture() {
            if (texture) {
                texture.Release();
            }
        }

        void SetShaderParameters() {
            raytracer.SetTexture(0, "Result", texture);
            raytracer.SetMatrix("_CameraToWorld", sourceCamera.cameraToWorldMatrix);
            raytracer.SetMatrix("_CameraInverseProjection", sourceCamera.projectionMatrix.inverse);
        }

        void Render() {
            var threadGroups = new Vector3Int(
                 Mathf.CeilToInt(Screen.width / 8.0f),
                 Mathf.CeilToInt(Screen.height / 8.0f),
                 1
            );

            raytracer.Dispatch(0, threadGroups.x, threadGroups.y, threadGroups.z);
        }

        void InitTexture() {
            if (!texture || texture.width != Screen.width || texture.height != Screen.height) {
                DisposeTexture();

                texture = new RenderTexture(
                    Screen.width,
                    Screen.height,
                    0,
                    RenderTextureFormat.ARGBFloat,
                    RenderTextureReadWrite.Linear) {
                    enableRandomWrite = true
                };

                texture.Create();

                //sourceCamera.targetTexture = texture;
                targetImage.texture = texture;
            }
        }
    }
}