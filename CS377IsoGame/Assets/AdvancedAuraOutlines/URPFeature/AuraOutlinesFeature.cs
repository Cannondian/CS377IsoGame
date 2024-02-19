using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


namespace AuraOutlines
{
    public class AuraOutlinesFeature : ScriptableRendererFeature
    {
        private RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingSkybox;

        private enum OutlineType
        {
            PencilContour, InkOutline, BrushOutline, AuraOutline, BlurOutline
        }

        [SerializeField,Range(1,5)] private int ID = 1;
        [SerializeField] private LayerMask layerMask = ~0;
        [SerializeField] private bool UseDepthMask = true;
        //TODO: UseBehindWallColor
        [HideInInspector] private bool UseBehindWallColor = false;

        [SerializeField] public AuraOutlinesSettings auraOutlinesSettings;

        private ObjectsPass objectsPass;
        private MaskPass maskPass;

        private AuraPass auraPass;

        public override void Create()
        {
            if (UseDepthMask) maskPass = new MaskPass(renderPassEvent, layerMask, ID);
            objectsPass = new ObjectsPass(renderPassEvent, layerMask, UseDepthMask, UseBehindWallColor, ID);
            auraPass = new AuraPass(renderPassEvent, auraOutlinesSettings, ID);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (UseDepthMask) renderer.EnqueuePass(maskPass);
            renderer.EnqueuePass(objectsPass);
            renderer.EnqueuePass(auraPass);
        }

        
    }
}