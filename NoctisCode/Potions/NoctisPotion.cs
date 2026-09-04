using BaseLib.Abstracts;
using BaseLib.Utils;
using Noctis.NoctisCode.Character;

namespace Noctis.NoctisCode.Potions;

[Pool(typeof(NoctisPotionPool))]
public abstract class NoctisPotion : CustomPotionModel;