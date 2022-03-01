using Telegram.Bot;
using Telegram.Bot.Types;

TelegramBotClient ManagerBot;
string PathCd = "";
async Task Run()
{
    ManagerBot = new TelegramBotClient("5238996973:AAHTtSC5R7dOaqINazUcXVxdsTtjcHRPskA");
    Console.WriteLine("Bot Running...");

    int Counter = 0;

    while (true)
    {
        var updates = ManagerBot.GetUpdatesAsync(Counter).Result;
        foreach (var item in updates)
        {
            Counter = item.Id + 1;
            long ChatId = 1865608858;
            try
            {
                if (item.Message != null)
                {
                    string? Command = item.Message.Text;
                    string StartupLocation = Environment.CurrentDirectory + "\\Data";
                    if (!string.IsNullOrEmpty(Command))
                    {
                        string Result = string.Empty;
                        if (Command == "/start")
                        {
                            await ManagerBot.SendTextMessageAsync(ChatId, $"Hello🖤\n/md: Create Dirctory📂\n/ls Data: List Dirctory Root📄\n/ls NameDirctory: List Files DirctoryCurrent📄");
                        }
                        else
                        {
                            if (Command.StartsWith("/md"))
                            {
                                string FolderName = Command.Replace("/md ", string.Empty);
                                if (!Directory.Exists(StartupLocation + "\\" + FolderName))
                                {
                                    Directory.CreateDirectory(StartupLocation + "\\" + FolderName);
                                    await ManagerBot.SendTextMessageAsync(ChatId, "Dirctory Created 📂");
                                }
                                else
                                {
                                    await ManagerBot.SendTextMessageAsync(ChatId, "Dirctory is available ❌");
                                }
                            }
                            if (Command.Equals("/ls Data"))
                            {
                                string[] Dirctorys = Directory.GetDirectories(StartupLocation);
                                if (Dirctorys.Length == 0)
                                {
                                    Result = "Empty Dirctory Create /md Dirctory Name.";
                                }
                                else
                                {
                                    Result += "Dirctory Data: \n";
                                    foreach (string Folder in Dirctorys)
                                    {
                                        if (!string.IsNullOrEmpty(Folder))
                                        {
                                            Result += "📂 " + Folder.Replace($"{Environment.CurrentDirectory}\\Data\\", "") + "\n";
                                        }
                                    }
                                }
                                await ManagerBot.SendTextMessageAsync(ChatId, Result);
                                Result = string.Empty;
                            }
                            else if (Command.StartsWith("/ls"))
                            {
                                string FolderName = "Data\\" + Command.Replace("/ls ", string.Empty);
                                string[] FilesDirctory = Directory.GetFiles(FolderName);
                                if (FilesDirctory.Length == 0)
                                {
                                    Result = $"Dirctory {Command.Replace("/ls ", string.Empty)} Is Empty /cd Upload Data.";
                                }
                                else
                                {
                                    Result += $"Dirctory {Command.Replace("/ls ", string.Empty)} Files: \n";
                                    foreach (string Files in FilesDirctory)
                                    {
                                        Result += "📄 " + Files + "\n";
                                    }
                                }
                                await ManagerBot.SendTextMessageAsync(ChatId, Result);
                                Result = string.Empty;
                            }
                            else if (Command.StartsWith("/open"))
                            {
                                PathCd = Command.Replace("/open ", string.Empty);
                                await ManagerBot.SendTextMessageAsync(ChatId, $"Dirctory {PathCd} Open:📂");
                            }
                            else if (Command.StartsWith("/cd."))
                            {
                                PathCd = StartupLocation;
                                await ManagerBot.SendTextMessageAsync(ChatId, $" Back Dirctory Data.✅");
                            }
                            else if (Command.StartsWith("/send"))
                            {
                                string PathFile = Command.Replace("/send ", "");
                                FileStream Files = System.IO.File.Open(PathFile, FileMode.Open);
                                if (Files.Name.EndsWith(".jpg"))
                                {
                                    await ManagerBot.SendTextMessageAsync(ChatId, $"Plase Wite...");
                                    await ManagerBot.SendPhotoAsync(ChatId, Files);
                                    await ManagerBot.SendTextMessageAsync(ChatId, $"Done.✅");
                                }
                                else if (Files.Name.EndsWith(".mp3"))
                                {
                                    await ManagerBot.SendTextMessageAsync(ChatId, $"Plase Wite...");
                                    await ManagerBot.SendAudioAsync(ChatId, Files);
                                    await ManagerBot.SendTextMessageAsync(ChatId, $"Done.✅");
                                }
                                else if (Files.Name.EndsWith(".oga"))
                                {
                                    await ManagerBot.SendTextMessageAsync(ChatId, $"Plase Wite...");
                                    await ManagerBot.SendVoiceAsync(ChatId, Files);
                                    await ManagerBot.SendTextMessageAsync(ChatId, $"Done.✅");
                                }
                                else if (Files.Name.EndsWith(".mp4"))
                                {
                                    await ManagerBot.SendTextMessageAsync(ChatId, $"Plase Wite...");
                                    await ManagerBot.SendVideoAsync(ChatId, Files);
                                    await ManagerBot.SendTextMessageAsync(ChatId, $"Done.✅");
                                }
                            }
                            else if (Command.StartsWith("/rmdir"))
                            {
                                string PathFolder = Command.Replace("/rmdir ", "");
                                string[] DirctoryGet = Directory.GetDirectories(StartupLocation);
                                foreach (var Folder in DirctoryGet)
                                {
                                    if (Folder.ToLower().EndsWith(PathFolder.ToLower()))
                                    {
                                        Directory.Delete(Folder, true);
                                        await ManagerBot.SendTextMessageAsync(ChatId, $"Delete Folder.✅");
                                    }
                                }
                            }
                            else if (Command.StartsWith("/del"))
                            {
                                string PathFolder = Command.Replace("/del ", "");
                                System.IO.File.Delete(PathFolder);
                                await ManagerBot.SendTextMessageAsync(ChatId, $"Delete Files.✅");
                            }
                        }
                    }
                    else
                    {
                        if (item.Message.Audio != null || item.Message.Voice != null || item.Message.Photo != null || item.Message.Video != null)
                        {
                            string[] DirctoryCheck = Directory.GetDirectories(StartupLocation);
                            if (PathCd == StartupLocation || PathCd == "" || DirctoryCheck.Length == 0)
                            {
                                await ManagerBot.SendTextMessageAsync(ChatId, "Empty Dirctory Create /md Dirctory Name First Open Or Create Dirctory❌");
                            }
                            else if (item.Message.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
                            {
                                string FolderName = "Data\\" + PathCd;
                                var GetFileInBot = await ManagerBot.GetFileAsync(item.Message.Photo.LastOrDefault()?.FileId);
                                var GetFileName = GetFileInBot.FilePath.Replace("photos/", "");
                                using (var Download = System.IO.File.Open(FolderName + "\\" + GetFileName, FileMode.Create))
                                {
                                    await ManagerBot.DownloadFileAsync(GetFileInBot.FilePath, Download);
                                }
                                await ManagerBot.SendTextMessageAsync(ChatId, "Photo Saved Dirctory Closed.✅");
                            }
                            else if (item.Message.Type == Telegram.Bot.Types.Enums.MessageType.Voice)
                            {
                                string FolderName = "Data\\" + PathCd;
                                var GetFileInBot = await ManagerBot.GetFileAsync(item.Message.Voice.FileId);
                                var GetFileName = GetFileInBot.FilePath.Replace("voice/", "");
                                using (var Download = System.IO.File.Open(FolderName + "\\" + GetFileName, FileMode.Create))
                                {
                                    await ManagerBot.DownloadFileAsync(GetFileInBot.FilePath, Download);
                                }
                                await ManagerBot.SendTextMessageAsync(ChatId, "Voiace Saved Dirctory Closed.✅");
                            }
                            else if (item.Message.Type == Telegram.Bot.Types.Enums.MessageType.Video)
                            {
                                string FolderName = "Data\\" + PathCd;
                                var GetFileInBot = await ManagerBot.GetFileAsync(item.Message.Video.FileId);
                                var GetFileName = GetFileInBot.FilePath.Replace("videos/", "");
                                using (var Download = System.IO.File.Open(FolderName + "\\" + GetFileName, FileMode.Create))
                                {
                                    await ManagerBot.DownloadFileAsync(GetFileInBot.FilePath, Download);
                                }
                                await ManagerBot.SendTextMessageAsync(ChatId, "Video Saved Dirctory Closed.✅");
                            }
                            else if (item.Message.Type == Telegram.Bot.Types.Enums.MessageType.Audio)
                            {
                                string FolderName = "Data\\" + PathCd;
                                var GetFileInBot = await ManagerBot.GetFileAsync(item.Message.Audio.FileId);
                                var GetFileName = GetFileInBot.FilePath.Replace("music/", "");
                                using (var Download = System.IO.File.Open(FolderName + "\\" + GetFileName, FileMode.Create))
                                {
                                    await ManagerBot.DownloadFileAsync(GetFileInBot.FilePath, Download);
                                }
                                await ManagerBot.SendTextMessageAsync(ChatId, "Audio Saved Dirctory Closed.✅");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Could not find a part of the path"))
                {
                    await ManagerBot.SendTextMessageAsync(ChatId, $"❌Error❌\nFile or Dirctory 404 Not Found.");
                }
                if (ex.Message.Contains("Could not find file"))
                {
                    await ManagerBot.SendTextMessageAsync(ChatId, $"❌Error❌\nFile or Dirctory 404 Not Found.");
                }
            }
        }
    }
}
await Run();