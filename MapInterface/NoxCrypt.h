/* 
 * Copyright (C) 2007 NoxForum.net <http://www.noxforum.net/>
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

#ifndef _NOXCRYPT_H
#define _NOXCRYPT_H

#include <iostream>
#include <fstream>

using namespace std;

enum NoxCryptFileTypes
{
	NC_PLAYER = 0x1,
	NC_MAP,
	NC_THING_BIN,
	NC_MODIFIER_BIN,
	NC_GAMEDATA_BIN,
	NC_MONSTER_BIN,
	NC_SOUNDSET_BIN
};


class NoxCrypt
{
public:
	static void decrypt_bitwise(unsigned char *data, size_t dataLen)
	{
		DecryptBitwise(data, dataLen);
	}
	static void encrypt_bitwise(unsigned char *data, size_t dataLen)
	{
		EncryptBitwise(data, dataLen);
	}
	static int decrypt(unsigned char *data, size_t length, NoxCryptFileTypes fileType)
	{
		return NoxCrypt_crypt(data, length, fileType, 1);
	}
	static int encrypt(unsigned char *data, size_t length, NoxCryptFileTypes fileType)
	{
		return NoxCrypt_crypt(data, length, fileType, 0);
	}
private:
	static int NoxCrypt_crypt(unsigned char *data, size_t BuffLen, int tabl, int mode);
	static void DecryptBitwise(unsigned char *data, size_t dataLen);
	static void EncryptBitwise(unsigned char *data, size_t dataLen);
};

#endif