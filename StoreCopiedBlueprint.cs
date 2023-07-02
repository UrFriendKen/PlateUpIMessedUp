using Kitchen;
using KitchenMods;
using Unity.Entities;

namespace KitchenIMessedUp
{
    [UpdateBefore(typeof(PickUpAndDropAppliance))]
    public class StoreCopiedBlueprint : ApplianceInteractionSystem, IModSystem
    {
        private CItemHolder Holder;

        private CApplianceBlueprint Blueprint;

        private CForSale Sale;

        private CAppliance Appliance;

        private CBlueprintStore Store;

        protected override InteractionType RequiredType => InteractionType.Grab;

        protected override bool IsPossible(ref InteractionData data)
        {
            if (!Require<CItemHolder>(data.Interactor, out Holder))
            {
                return false;
            }
            if (!Require<CApplianceBlueprint>((Entity)Holder, out Blueprint))
            {
                return false;
            }
            if (!Require<CForSale>((Entity)Holder, out Sale))
            {
                return false;
            }
            if (!Require<CAppliance>((Entity)Holder, out Appliance))
            {
                return false;
            }
            if (!Require<CBlueprintStore>(data.Target, out Store))
            {
                return false;
            }
            if (!Blueprint.IsCopy)
            {
                return false;
            }
            if (!Store.InUse)
            {
                return false;
            }
            if (Store.HasBeenCopied)
            {
                return false;
            }
            return true;
        }

        protected override void Perform(ref InteractionData data)
        {
            Main.LogInfo("StoreCopiedBlueprint.Perform");
            if (Store.Price == Sale.Price && Store.ApplianceID == Blueprint.Appliance)
            {
                data.Context.Destroy(Holder.HeldItem);
                data.Context.Set(data.Interactor, default(CItemHolder));
                Store.HasBeenCopied = true;
                SetComponent(data.Target, Store);
            }
        }
    }
}
