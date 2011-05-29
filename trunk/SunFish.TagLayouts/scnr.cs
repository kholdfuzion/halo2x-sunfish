using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunfish.TagLayouts
{
    public class scnr : TagDefinition
    {
        public scnr()
        {
            Size = 992;
            Values = new Value[] {
                new Data(8),
                new TagBlockArray(new TagBlock(8, null)),
                new Data(16),
                new TagBlockArray(new TagBlock(8, null)),
                new Data(32),
                new TagBlockArray(new TagBlock(36, null)),
                new TagBlockArray(new TagBlock(92, null)),
                new TagBlockArray(new TagBlock(40, null)),
                new TagBlockArray(new TagBlock(84, null)),
                new TagBlockArray(new TagBlock(40, null)),
                new TagBlockArray(new TagBlock(84, null)),
                new TagBlockArray(new TagBlock(40, null)),
                new TagBlockArray(new TagBlock(56, null)),
                new TagBlockArray(new TagBlock(40, null)),
                new TagBlockArray(new TagBlock(84, null)),
                new TagBlockArray(new TagBlock(40, null)),
                new TagBlockArray(new TagBlock(40, null)),
                new TagBlockArray(new TagBlock(72, null)),
                new TagBlockArray(new TagBlock(40, null)),
                new TagBlockArray(new TagBlock(68, null)),
                new TagBlockArray(new TagBlock(40, null)),
                new Data(16),
                new TagBlockArray(new TagBlock(80, null)),
                new TagBlockArray(new TagBlock(40, null)),
                new TagBlockArray(new TagBlock(108, null)),
                new TagBlockArray(new TagBlock(40, null)),
                new TagBlockArray(new TagBlock(68, null)),
                new TagBlockArray(new TagBlock(52, null)),
                new TagBlockArray(new TagBlock(68, null)),
                new Data(8),
                new TagBlockArray(new TagBlock(32, null)),
                new TagBlockArray(new TagBlock(144, null)),
                new TagBlockArray(new TagBlock(156, null)),
                new TagBlockArray(new TagBlock(14, null)),
                new TagBlockArray(new TagBlock(8, null)),
                new Data(8),
                new TagBlockArray(new TagBlock(8, null)),
                new TagBlockArray(new TagBlock(36, null)),
                new TagBlockArray(new TagBlock(116, new Value[] {
                    new Data(72),
                    new TagBlockArray(new TagBlock(100, null)),
                })),
                new TagBlockArray(new TagBlock(56, new Value[] {
                    new Data(40),
                    new TagBlockArray(new TagBlock(32, null)),
                    new TagBlockArray(new TagBlock(136, new Value[] {
                        new Data(128),
                        new TagBlockArray(new TagBlock(4, null)),
                    })),
                })),                
                new TagBlockArray(new TagBlock(24, new Value[] {
                    new Data(8),
                    new TagBlockArray(new TagBlock(12, new Value[] {
                        new Data(4),
                        new TagBlockArray(new TagBlock(8, null)),
                    })),
                    new TagBlockArray(new TagBlock(16, new Value[] {
                        new Data(8),
                        new TagBlockArray(new TagBlock(4, null)),
                    })),
                })),    
                new TagBlockArray(new TagBlock(8, null)), 
                new Data(48),
                new TagBlockArray(new TagBlock(1, null)),
                new TagBlockArray(new TagBlock(40, null)),
                new TagBlockArray(new TagBlock(40, null)),
                new TagBlockArray(new TagBlock(8, null)),
                new Data(8),
                new TagBlockArray(new TagBlock(128, new Value[] {
                    new TagBlockArray(new TagBlock(48, new Value[] {
                        new Data(32),
                        new TagBlockArray(new TagBlock(76, null)),
                    })),
                })),   
                new TagBlockArray(new TagBlock(56, null)),
                new TagBlockArray(new TagBlock(64, null)),
                new TagBlockArray(new TagBlock(36, null)),
                new TagBlockArray(new TagBlock(68, null)),
                new Data(32),
                new TagBlockArray(new TagBlock(24, new Value[] {
                    new TagBlockArray(new TagBlock(8, null)),
                    new TagBlockArray(new TagBlock(8, null)),
                    new TagBlockArray(new TagBlock(8, null)),
                })),
                new Data(8),
                new TagBlockArray(new TagBlock(8, null)),
                new TagBlockArray(new TagBlock(2, null)),
                new TagBlockArray(new TagBlock(20, null)),
                new TagBlockArray(new TagBlock(68, new Value[] {
                    new Data(84),
                    new TagBlockArray(new TagBlock(8, null)),
                    new TagBlockArray(new TagBlock(8, null)),
                    new TagBlockArray(new TagBlock(12, new Value[] {
                        new Data(4),
                        new TagBlockArray(new TagBlock(8, null)),
                    })),
                    new TagBlockArray(new TagBlock(4, null)),
                    new TagBlockArray(new TagBlock(20,new Value[] {
                        new Data(12),
                        new TagBlockArray(new TagBlock(8, null)),
                    })),
                })),     
                new TagBlockArray(new TagBlock(48, new Value[] {
                    new Data(40),
                    new TagBlockArray(new TagBlock(56, null)),
                })),          
                new TagBlockArray(new TagBlock(100, null)), 
                new TagBlockArray(new TagBlock(72, null)),   
                new TagBlockArray(new TagBlock(136, null)),                
                new Data(40),
                new TagBlockArray(new TagBlock(52, new Value[] {
                    new Data(8),
                    new TagBlockArray(new TagBlock(4, null)),
                    new TagBlockArray(new TagBlock(4, null)),
                    new Data(4),
                    new TagBlockArray(new TagBlock(12, null)),
                    new TagBlockArray(new TagBlock(4, null)),
                    new TagBlockArray(new TagBlock(4, null)),
                })),              
                new Data(128),
                new TagBlockArray(new TagBlock(96, new Value[] {
                    new Data(72),
                    new TagBlockArray(new TagBlock(16, null)),
                    new TagBlockArray(new TagBlock(48, null)),
                    new TagBlockArray(new TagBlock(48, null)),
                })), 
                new Data(8),
                new TagBlockArray(new TagBlock(76, null)),  
                new TagBlockArray(new TagBlock(40, null)),  
                new Data(8),
                new TagBlockArray(new TagBlock(244, null)),  
                new TagBlockArray(new TagBlock(16, null)),  
                new Data(40),
                new TagBlockArray(new TagBlock(8, null)),  
                new Data(8),
                new TagBlockArray(new TagBlock(16, new Value[] {
                    new Data(8),
                    new TagBlockArray(new TagBlock(12, null)),
                })),     
                new Data(8),
                new TagBlockArray(new TagBlock(24, new Value[] {
                    new Data(8),
                    new TagBlockArray(new TagBlock(2896, null)),
                    new TagBlockArray(new TagBlock(3172, null)),
                })),     
                new Data(16),
                new TagBlockArray(new TagBlock(8, null)),   
                new Data(8),
                new TagBlockArray(new TagBlock(24, new Value[] {
                    new Data(12),
                    new TagBlockArray(new TagBlock(1, null)),
                })), 
                new Data(8),
                new TagBlockArray(new TagBlock(36, null)), 
                new TagBlockArray(new TagBlock(4, null)), 
            };
        }
    }
}