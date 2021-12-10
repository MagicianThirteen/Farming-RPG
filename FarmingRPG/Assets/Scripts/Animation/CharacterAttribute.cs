
//这里这个是为了后面穿个类似的参数，可以让动画控制器知道要替换的是哪一部分
public struct CharacterAttribute
{
   public CharacterPartAnimator characterPart;
   public PartVariantColour partVariantColour;
   public PartVariantType partVariantType;

   public CharacterAttribute(CharacterPartAnimator _characterPartAnimator, PartVariantColour _partVariantColour,
      PartVariantType _partVariantType)
   {
      characterPart = _characterPartAnimator;
      partVariantColour = _partVariantColour;
      partVariantType = _partVariantType;
   }
}
