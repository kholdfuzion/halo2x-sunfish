
namespace Sunfish.TagLayouts
{
    public class fx : TagDefinition
    {
        public fx()
        {
            Size=28;
            Values = new Value[] {
                new Data(12),
                new TagBlockArray(new TagBlock(16, new Value[] {
                    new StringID(),
                    new TagBlockArray(new TagBlock(1, null)), 
                })), 
                new TagBlockArray(new TagBlock(12, new Value[] {
                    new TagBlockArray(new TagBlock(24, new Value[] {                            
                        new StringID(),
                        new Data(12),
                        new TagBlockArray(new TagBlock(16, null)),
                    }))
                })),
            };
        }
    }
}