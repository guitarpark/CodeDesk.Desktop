#import "macOSLibrary.h"
#import <Cocoa/Cocoa.h>
@implementation macOSLibrary
void Create1()
{
    // 初始化应用程序对象
    NSApplication *application = [NSApplication sharedApplication];
    
    // 创建窗口对象
    NSRect frame = NSMakeRect(0, 0, 500, 500);
    NSWindow *window = [[NSWindow alloc] initWithContentRect:frame
                                                   styleMask:NSWindowStyleMaskTitled | NSWindowStyleMaskClosable | NSWindowStyleMaskResizable
                                                     backing:NSBackingStoreBuffered
                                                       defer:NO];
    
    // 设置窗口标题
    [window setTitle:@"My Window"];
    
    // 设置窗口为主窗口
    [application setActivationPolicy:NSApplicationActivationPolicyRegular];
    [application activateIgnoringOtherApps:YES];
   
    
    // 显示窗口
    [window makeKeyAndOrderFront:nil];
    
    // 运行应用程序
    [application run];
    
}
@end
