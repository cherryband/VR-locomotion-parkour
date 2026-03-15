---
title: "Implementation - Preparation"
slug: implementation-preparation
date: 2026-03-15T13:00:00+01:00
---

The implementation was split between 3 stages: one each for locomotion and interaction, and one for miscellaneous improvements/tidying up.

Except there was one problem needed solving before the development could even begin.

# Windows Installation

Meta Quest SDK only works on Windows, which I was informed of during the class introduction. My system was running Linux. It did not have a working Windows installation and it hasn't for a long time.

Installing Windows itself is relatively easy (even though it requires [extra steps](https://pureinfotech.com/bypass-microsoft-account-setup-windows-11/) nowadays). The problem lied on the fact that my storage was using a [LUKS on LVM](https://wiki.archlinux.org/title/Dm-crypt/Encrypting_an_entire_system#LUKS_on_LVM) configuration. In my case, a LUKS-encrypted partition was span across 2 different SSDs using LVM, effectively creating a RAID 0 configuration.

![diagram of the storage madness](../diagram4.png)

A space to install Windows had to come from shrinking the LUKS on LVM partition taking up the entire storage. Resizing LUKS on LVM is as convoluted as the storage situation itself, shrinking it riskier than growing. It has to be done manually because almost all partition software tools don't understand this setup, and to this day I am unsure if I can perform it successfully. It has 3 separate failure points (the encryption, the LVM, the partition itself), so I wanted to avoid the procedure if at all possible.
Worse, I did not have a lot of free space, which is why it was on effectively RAID 0 in the first place. So even if I had shrunk the Linux partition to install Windows, it wouldn't have been large enough to even install Unity.

The logistical complexity of *preparing* to install Windows and a lack of extra storage made the prospect of developing on Linux more enticing. For this to happen, however, one of two requirements had to be fulfilled.

1. Develop with an alternative SDK, notably SteamVR. This enables native Linux development, but I will have to port the example project on my own, *on top of* implementing my own locomotion/interaction. Due to my lack of experience across Unity and SteamVR, I have decided this was not worth pursueing.
2. Run Windows virtually. This seemed promising, and I have managed to run Unity inside a virtual machine. However, the fact that it runs virtually meant that performance, especially graphics, was noticeably slow. Considering I am developing a 3D game, the lack of graphics acceleration proved to be untenable. I have tried several different virtualisation solutions(QEMU, VirtualBox, [WinBoat](https://www.winboat.app/) which uses Docker, etc.) to no avail. None of them supported graphics acceleration out of the box, and procedures such as [GPU passthrough](https://gist.github.com/firelightning13/e530aec3e3a4e15885a10f6c4b7ae021) is tricky on its own right, and not having a dedicated GPU made it even harder if not impossible.

This dilemma was resolved only after purchasing a new SSD with much more capacity. The decision to do this wasn't necessarily made *because* of this issue, but it was a contributing factor. It took me a full day to copy over the data. The bottleneck was ensuring the configurations were all correct and I wasn't losing data, rather than the drive speed which was as fast as it can be. With the newly available space, however, the RAID configuration became unnecessary, therefore the storage management became easier. Enough storage was finally allocated to a bare-metal Windows partition. This entire process of research and trial-and-error took roughly a month.
