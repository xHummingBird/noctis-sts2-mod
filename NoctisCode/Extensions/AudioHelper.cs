using MegaCrit.Sts2.Core.Commands;

namespace Noctis.NoctisCode.Extensions
{
    public static class AudioHelper
    {
        private static readonly Random rng = new Random();

        private static readonly string[] attackSfx =
        {
            "res://Noctis/sounds/attack (1).wav",
            "res://Noctis/sounds/attack (2).wav",
            "res://Noctis/sounds/attack (3).wav",
            "res://Noctis/sounds/attack (4).wav",
            "res://Noctis/sounds/attack (5).wav",
            "res://Noctis/sounds/attack (6).wav",
        };
    
        private static readonly string[] attackMediumSfx =
        {
            "res://Noctis/sounds/attack_medium (1).wav",
            "res://Noctis/sounds/attack_medium (2).wav",
            "res://Noctis/sounds/attack_medium (3).wav",
            "res://Noctis/sounds/attack_medium (4).wav",
        
        };
        
        private static readonly string[] defendSfx =
        {
            "res://Noctis/sounds/defend (1).wav",
            "res://Noctis/sounds/defend (2).wav",
            "res://Noctis/sounds/defend (3).wav",
            "res://Noctis/sounds/defend (4).wav",
            "res://Noctis/sounds/defend (5).wav",
            "res://Noctis/sounds/defend (6).wav",
            
        };
    
        private static readonly string[] damagedSfx =
        {
            "res://Noctis/sounds/hurt (1).wav",
            "res://Noctis/sounds/hurt (2).wav",
            "res://Noctis/sounds/hurt (3).wav"
        };
    
        private static readonly string[] highDamagedSfx =
        {
            "res://Noctis/sounds/hurt_high (1).wav",
            "res://Noctis/sounds/hurt_high (2).wav",
            "res://Noctis/sounds/hurt_high (3).wav"
        };
    
        private static readonly string[] criticalDamagedSfx =
        {
            "res://Noctis/sounds/hurt_critical (1).wav",
            "res://Noctis/sounds/hurt_critical (2).wav",
            "res://Noctis/sounds/hurt_critical (3).wav"
        };
    
        private static readonly string[] attackCriticalSfx =
        {
            "res://Noctis/sounds/attack_critical (1).wav",
            "res://Noctis/sounds/attack_critical (2).wav",
            "res://Noctis/sounds/attack_critical (3).wav",
        };
    
        private static readonly string[] victorySfx =
        {
            "res://Noctis/sounds/victory (1).wav",
            "res://Noctis/sounds/victory (2).wav",
            "res://Noctis/sounds/victory (3).wav",
            "res://Noctis/sounds/victory (4).wav"
        };

        private static readonly string[] gameoverSfx =
        {
            "res://Noctis/sounds/gameover (1).wav",
            "res://Noctis/sounds/gameover (2).wav",
        };
    
        private static readonly string[] phraseSfx =
        {
            "res://Noctis/sounds/phrase (1).wav",
            "res://Noctis/sounds/phrase (2).wav",
            "res://Noctis/sounds/phrase (3).wav",
            "res://Noctis/sounds/phrase (4).wav",
            "res://Noctis/sounds/phrase (5).wav",
            "res://Noctis/sounds/phrase (6).wav",
            "res://Noctis/sounds/lasthit_2 (1).wav",
            "res://Noctis/sounds/lasthit_2 (2).wav",
            "res://Noctis/sounds/lasthit_2 (3).wav",
            "res://Noctis/sounds/lasthit_2 (4).wav",
            "res://Noctis/sounds/lasthit_2 (5).wav",
            "res://Noctis/sounds/lasthit_2 (6).wav",
            "res://Noctis/sounds/lasthit_2 (7).wav",
            "res://Noctis/sounds/lasthit_2 (8).wav",
            "res://Noctis/sounds/lasthit_2 (9).wav",
        };

        private static readonly string[] attackHighSfx =
        {
            "res://Noctis/sounds/attack_high (1).wav",
            "res://Noctis/sounds/attack_high (2).wav",
            "res://Noctis/sounds/attack_high (3).wav",
            "res://Noctis/sounds/attack_high (4).wav"
        };
    
        private static readonly string[] lastHitSfx =
        {
            "res://Noctis/sounds/lasthit (1).wav",
            "res://Noctis/sounds/lasthit (2).wav",
            "res://Noctis/sounds/lasthit (3).wav",
            "res://Noctis/sounds/lasthit (4).wav"
        };
        
        private static readonly string[] lastHit2Sfx =
        {
            "res://Noctis/sounds/lasthit_2 (1).wav",
            "res://Noctis/sounds/lasthit_2 (2).wav",
            "res://Noctis/sounds/lasthit_2 (3).wav",
            "res://Noctis/sounds/lasthit_2 (4).wav",
            "res://Noctis/sounds/lasthit_2 (5).wav",
            "res://Noctis/sounds/lasthit_2 (6).wav",
            "res://Noctis/sounds/lasthit_2 (7).wav",
            "res://Noctis/sounds/lasthit_2 (8).wav",
            "res://Noctis/sounds/lasthit_2 (9).wav",
            
        };
        
        private static readonly string[] limitBreakSfx =
        {
            "res://Noctis/sounds/transform (1).wav",
            "res://Noctis/sounds/transform (2).wav",
            "res://Noctis/sounds/transform (3).wav",
            "res://Noctis/sounds/transform (4).wav",
        };
    
        public static void PlayRandomAttack()
        {
            PlayRandom(attackSfx);
        }
        
        public static void PlayRandomDefend()
        {
            PlayRandom(defendSfx);
        }
    
        public static void PlayRandomAttackCritical()
        {
            PlayRandom(attackCriticalSfx);
        }

        public static void PlayRandomAttackMedium()
        {
            PlayRandom(attackMediumSfx);
        }
    
        public static void PlayRandomDamaged()
        {
            PlayRandom(damagedSfx);
        }

        public static void PlayRandomDamagedHigh()
        {
            PlayRandom(highDamagedSfx);
        }

        public static void PlayRandomGameover()
        {
            PlayRandom(gameoverSfx);
        }

        public static void PlayRandomDamagedCritical()
        {
            PlayRandom(criticalDamagedSfx);
        }
    
        public static void PlayRandomVictory()
        {
            PlayRandom(victorySfx);
        }

        public static void PlayRandomAttackHard()
        {
            PlayRandom(attackHighSfx);
        }

        public static void PlayRandomLastHit()
        {
            PlayRandom(lastHitSfx);
        }

        public static void PlayRandomLimitBreak()
        {
            PlayRandom(limitBreakSfx);
        }

        public static void PlayRandomLastHit1()
        {
            PlayRandom(lastHitSfx);
        }
    
        public static void PlayRandomLastHit2()
        {
            PlayRandom(lastHit2Sfx);
        }
        
        public static void PlayRandomPhrase()
        {
            PlayRandom(phraseSfx);
        }

        public static void PlayRandom(string[] pool)
        {
            int index = rng.Next(pool.Length);
            SfxCmd.Play(pool[index]);
        }
    }
}
