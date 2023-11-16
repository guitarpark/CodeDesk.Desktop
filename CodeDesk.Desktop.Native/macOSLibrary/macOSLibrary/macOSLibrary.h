#import <Foundation/Foundation.h>
#import <Cocoa/Cocoa.h>

@interface macOSLibrary : NSObject
NSWindow *Create(int left,int top,int width,int height,bool chromeless,bool center);
void SetTitle(NSWindow *window,char title[]);
void Show(NSWindow *window);
void RunMessageLoop();
@end
