#if !defined(_CRT_SECURE_NO_WARNINGS)
#define _CRT_SECURE_NO_WARNINGS
#endif

#include <discordias/discord.h>
namespace Windows
{
	#include <Windows.h>
}

// I need to be more...profesional
#define var auto // auto type
#define ref & // Reference to lvalue
#define ignore (void) // Ignore return value

#define WindowsApi ::Windows::
#define DiscordApi ::discord:: // Discord namespace

int main()
{
	DiscordApi ClientId id;
	FILE *fs = fopen("src/secret.txt", "r");
	ignore fscanf(fs, "%lli", &id);
	fclose(fs);

	DiscordApi Core *instance;
	ignore DiscordApi Core::Create(id, DiscordCreateFlags_Default, ref instance);
	DiscordApi Activity activity;
	activity.SetState("I amn't-ent human");
	activity.SetDetails("...or am I?");
	activity.SetType(DiscordApi ActivityType::Playing);
	instance->ActivityManager().UpdateActivity(activity, [](DiscordApi Result result) {});

	while (true)
	{
		if (WindowsApi GetAsyncKeyState(VK_ESCAPE) & 0x8000)
			break;
		ignore instance->RunCallbacks();
	}
}