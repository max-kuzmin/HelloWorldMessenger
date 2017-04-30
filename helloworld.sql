-- Adminer 4.2.4 MySQL dump

SET NAMES utf8;
SET time_zone = '+00:00';
SET foreign_key_checks = 0;
SET sql_mode = 'NO_AUTO_VALUE_ON_ZERO';

DROP TABLE IF EXISTS `dialog`;
CREATE TABLE `dialog` (
  `dialog_id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `time` int(11) NOT NULL,
  PRIMARY KEY (`dialog_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO `dialog` (`dialog_id`, `name`, `time`) VALUES
(18,	'Max-Ann',	1493469811),
(19,	'Friend Ivan',	1493469843),
(20,	'max1-monster77',	1493469874),
(21,	'LOL',	1493469955),
(22,	'Work time',	1493469913),
(23,	'dgfg',	1493552115),
(24,	'max1-ann',	1493552129),
(25,	'max1-ann',	1493554757),
(26,	'max1-ann',	1493555502),
(27,	'max1-ann',	1493555663),
(28,	'max1-ann',	1493555805),
(29,	'max1-ann',	1493555990),
(30,	'max1-ann',	1493556142),
(31,	'max1-ann',	1493556294),
(32,	'max1-ann',	1493556458),
(33,	'max1-ann',	1493556616),
(34,	'max1-ann',	1493556905),
(35,	'max1-ann',	1493559421),
(36,	'max1-ann',	1493559516),
(37,	'max1-ann',	1493559668),
(38,	'max1-ann',	1493559937),
(39,	'max1-ann',	1493560112),
(40,	'Dialog 7886722804',	1493560279),
(41,	'max1-ann',	1493560348),
(42,	'Example',	1493575631),
(43,	'max1-ann',	1493575732),
(44,	'max1-ann',	1493582318),
(45,	'max1-ann',	1493582571),
(46,	'max1-ann',	1493582454),
(47,	'Dialog 9594414132',	1493591986);

DROP TABLE IF EXISTS `image`;
CREATE TABLE `image` (
  `img_id` int(11) NOT NULL AUTO_INCREMENT,
  `img` mediumblob NOT NULL,
  `login` varchar(50) DEFAULT NULL,
  `message_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`img_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO `image` (`img_id`, `img`, `login`, `message_id`) VALUES
(6,	'ÿØÿà\0JFIF\0\0\0\0\0\0ÿÛ\0C\0\n\n\n\r\rÿÛ\0C		\r\rÿÀ\0\0d\0d\"\0ÿÄ\0\0\0\0\0\0\0\0\0\0\0	\nÿÄ\0µ\0\0\0}\0!1AQa\"q2‘¡#B±ÁRÑð$3br‚	\n\Z%&\'()*456789:CDEFGHIJSTUVWXYZcdefghijstuvwxyzƒ„…†‡ˆ‰Š’“”•–—˜™š¢£¤¥¦§¨©ª²³´µ¶·¸¹ºÂÃÄÅÆÇÈÉÊÒÓÔÕÖ×ØÙÚáâãäåæçèéêñòóôõö÷øùúÿÄ\0\0\0\0\0\0\0\0	\nÿÄ\0µ\0\0w\0!1AQaq\"2B‘¡±Á	#3RðbrÑ\n$4á%ñ\Z&\'()*56789:CDEFGHIJSTUVWXYZcdefghijstuvwxyz‚ƒ„…†‡ˆ‰Š’“”•–—˜™š¢£¤¥¦§¨©ª²³´µ¶·¸¹ºÂÃÄÅÆÇÈÉÊÒÓÔÕÖ×ØÙÚâãäåæçèéêòóôõö÷øùúÿÚ\0\0\0?\0÷ïx;Fð†ôÿ\0x{O‡LÒ,cAmÀÀêIêI9,Ç%‰$’I\'`1õÈëž™ÿ\0ëPG\'ŽR{ÿ\0õ©>½zäÿ\03þü·)JmÊNížx¡Ž=úäÿ\0:2}}ùþgü?ÈOÿ\0_?Ìÿ\0‡ùÿ\0¯ŸæÃü„äúûóüÏø}}ùþgü?ÈüüøÙû[ø«ÅÞ$¼Ó|5ªË¢xj9ÝmgÓ™¡žéPü²<„+‚Ü°A·‚	\\žOÂ?´¯ÄŸ^àñMî¥ÉËo«Èo#!?.dË 9 ì*Oä>î—cªQU¢¤þË¿âí¹Ð¨É«Ÿ¥û±Üþ?Ìÿ\0…#©#}¿úþÕðÖ¯ûyxÆæMÐô{û2¤òL%™ž~wHŸ:…^˜FŒrÍÛ¬ø1ûiêž#ñn™ x³L±Xµ	£´‡Q°ŠFÊ†tbÁ÷1AT.IÁ\n¼-™Ò¥*²‚Ó[]\\—JI\\úç\'žqÓüOùú™<óŒ~8ÿ\0þ~©ÓÛŽßñ?çêtöÇãñ?çëòF\"äóÎ1øãüOùú™<óŒ~8ÿ\0þ~©ÓÛŽ?Äÿ\0Ÿ©Óðàc×ÛÔûÿ\0õèÊ>#~Ê_¾,x–Ox—Ã	w«É\ZÃ-Í½ÌÖæP¹Á-Ô;\0q¸äáTg\n\0+Ö“§ÓŽ:\n+Ò†gŽ¥W’Kd¤ÿ\0Ì|Ï¸ÓÔþ|ÿ\03Iÿ\0ëçùŸðÿ\0!OSÓ×ŸæÏÿ\0Y?ý|ÿ\03þäy¢ÿ\0_?Ìÿ\0‡ùWÆ¿\'„¾ø»TkÉl%N™-î\"Ü$IäS$åO˜ÈíÔàŽ×ÿ\0×Ïó?áþG…þÚ:]Õÿ\0ÀÛÙíîY^Û\\\\Ç’Ñ–1…?ð9±þÏµzye(×ÆÑ¥7dä—ãú•y$~tÝ¥£‘ÌJNøîz„>„žŸÔ}d[©¢À¸·lçáùÔñ×Gåø×Þ^ý–|!ñ£à?„¯î´o¾›\ZkZ®ï0)p«4D…`ž£v+ÌÏüƒÄßÛØ:ß‡ÿ\0²„áMÏï¼ï\'v7y6ïÛÎÍøÏ»×ôq”[jNÍFðÒN*éŸ/Ãr“–»#‡B§¿¨éÅI_ªßþxà‡„?±t˜–{™Â¾¡¨Iu&äáFHTÉ\nòK37Sâ/øsÅÍ×¼?¥ëFÜ	Ôl£¸òCcp]êpNqè?G™EJÊ:±Ê¦ãw+>Ö>Fý~<½ü¿ð‚x‚îú÷QžW›K¼¸Ì,YxI<®Ñeê>f`úÛ§¶?Oþ¿ùúü{â+¾ÿ\0‚ƒ˜’fµež	w!ä„ÓQÊÿ\0À‚•?ïWØ]?éï_’qFlgI[ž*My¶×éùŸ1Š¦©U”@éœv8üz{Ñÿ\0êãùóÿ\0Ö?ý\\!þúÇÿ\0«ä?Çü9§O§tÔP>œqÐ{QHž§§¯?Ìÿ\0Ÿþ²úùþgü?ÈSÔôõçùŸóÿ\0ÖNŸÏŸæÃü†Ó?Ÿ?Ìÿ\0…pŸ<?Ž~x·J·ŠâYÚÌÜE´~d’Ë$aG,Y£UÀçæãµw3Î1ëÉúýj{yåDò¢á¼Ä9ë¸nëÔíÏùëêåJMÎV³Mz­¿»Jjñ§RVOóè|ûð»âÍ§†¾x\'ÃÚv¯äx†ßL·»¼·‹Ã:†¶míçF’xí0b2«#¡vî\0‚	èü;ñÏS¶ñV|Gªiv¾ŽÕžãVÔü5«øxÚË»™/  ’ªJ¬Kñ’6¿KðÛáõÃKkïéZGØ }Fÿ\0QŠkkà¶•%œL Ù”Òâ;ulvÌU6(5»¯i7:Ö­è°[ÛµãØ0QªÙ¼Ö,eYN‹(%NøÕ·#8Þ¥¿pöxyEÍËOøsìãíâ’íëÓçcñGÇßÞx~ö/øÛÂ\ZßŠ¼½öV¨}¼Éƒ™1¡yå`Û©<zd×1 |m×Êiòë7ò‡Ü†îÏOøeâ#Ÿc™ÿ\0ˆŒ€Í_áìu<û=øáŽ¥¡ë>Ñì­ÿ\0âIo¢ßÿ\0£B—S\"ÉreEPÒ’O<Áå¶A…RNúÖSw£EªGot¶Ò@\'Xä¶’9Â•Ýƒ |PÀäc<TQ¥†”>/¾ß©rxŽoy~vüÏœ|¡ÅãïÛ3Xñ•„Ww\Zh–ÚÆ—¨Ç¤I5´vñ¶æQ•l]ÓæÇü³a_MA Œòà!þúÜ×Ãß	Â3â/ëÓiqÙÝxQ[ƒ\'–‚&ÞÖÞÖ(ÝÐœ†hæ™9U›•W.Lî]ÙR‡ùóÿ\0ÖüÓŠ}›¯	)^VJÞKþ\rÏ—ÌèF“S“÷åwo+è7ÿ\0ÕÇòãþAÿ\0êãùñÿ\0 ÿ\0õqü‡øÿ\0ú¸þCüÈøƒÃ?Ã ¢„å}~(©§Œôõÿ\0ëŸóÿ\0Ö:gœc×“õúÐx\'¶;úõé:g¶?‰ÿ\0?VÓ=±øãüOùúº9\Z&§k/~»süÉÿ\0>íéíÇoøŸóõ:{cñÇøŸóõ¨ÉÁ©EÙ¡¦âÓ‹³Fê°ep1‘íŸêÏ¼÷‰§XÜÝH²˜íãiY`…ç\0	;c@YØöUž€y‡N¸ò[‚Ëßýóõ§âi5Ø-Vm\n;K™S!­nAÉÆ6¸`9ÎAàðF0ß®`±pÆPUcóò}Qúf¼qtã8¿_&Ma«ÚÎ4è,ã˜Å4Lëº	PD‰´bW	&]G–å[‡8ù\ZŸþ®?ÿ\0\Zó?\rø‡ân±y4\Z†¢èÑ.cbY§\nßîaÇðž}@äzL²¬(YŽ\0ãåÿ\0ÐGùÿ\0ëuÊq„\\¤ì‘×RÔ—4ž…MJb¸Œ9Çèåþ{gÿ\0ú¸þCüÈ|²dg=O/ò—_òÿ\0êãùñÿ\0#ò|~)âñ«Ó§§õ¯ÌüÓˆúÍyTéÓÓúÔ?ý\\!þ?äœ{ŽßÈœ{ŽßÈÚÏþ=ÿ\0Öÿ\0?_<âœŒýïz(NF~÷¹ïô¢¤ž§¶?Oþ¿ùú§Ol~;Äÿ\0Ÿªž§œcôÿ\0ëÿ\0ŸªtöÇãñ?çêÀ:{cñÇøŸóõØüqþ\'üýAÇ¶?‰ÿ\0?\\?xëÃþY?µ5{k9#PÍ™ºlˆ×,r{€qÏÖº(aëbf©Pƒ”ŸD›r*1”Ý¢®ÍÐ2@\0“Ðÿ\0=kUüøxR ío}§ü÷çÓÅ´¿ÚWÂíâak<7VúXFRt$àÔÚ@áºä€TG±i\Zþ™¯E$º^£i¨Çïe:Êã;r¤àÿ\0Ÿ§éyfEŽÊ©Ê®6“ƒ­~Þ~zìõ_3ír|$ðñ”ê+7ÓÈ©qâhâÊ¥¼ž`;J¿ËaþúÕmµo÷™OÌ§\0(Àû«ùuÿ\0!ž&·òïP\0.=Gaøb¼ÿ\0Ä´?A<¦îëØ·F,-¥ÙÇ3³©=1Ð‘ŠéÄåøŒÒœ°´\"ÜžÖý|½vÜö1¸e‰ÂÊÑôù“ÿ\0êãùñÿ\0 ú? ö÷¯5ð·íáoyq]Lú5ÓR—ƒ÷Eˆä,ƒ (?ÄÛsÇž‡a¨Zê¶©ues\rí´¹Ù4GÁ àŒŒÿ\0<þsÊ±Ùdù1”eU£ô{?“?7«B­j‘hŸ¯ûYÿ\0Ç¿úßçêuÿ\0k?ø÷ÿ\0[üýN¿ígÿ\0ÿ\0ëŸ©×ý¬ÿ\0ãßýoóõòŒ§#?{Ü÷úQBr3÷½Ïz*@iêyÆ?Oþ¿ùúùßÅ‹°ü9ŽK{a{«\\FÎ‘;á!^@wîÄ°û£Ãr8Ï¢§œcôÿ\0ëÿ\0Ÿ¯Æ?µ)µOˆž\"šp¡ÒòH\0Q–3å¯ãµkô~Èpùæc%‹W§M]®îöKN›·én§­–ác‰ªùöE¯|añg‰üÄ¸Õdµ¶bßèÖ?¹@ÁRGÌÃÙ‰®2Š+ú«	Âà)û,-8Â=¢’ü¶§N—,QEÚhQE\0oKÖ/ôYÚ}:úæÂg]%¬­2ç$¤q8öRŠ‰Â5\"á5tú2ZMYž¹áOÚC_Óî¢Mq!Õìš\\Í\"Ä#œ&1…Û…Àë‚9ÉÈú?HÕìõý6\rCO¸KË;•ß©ÑÇ#ê0r<ƒyëð¥}=û4\\Ëqðþæ9$iBHâV9\n»#lA–cõcëÏà¾ pÆ_ƒÁ,ÇMS’’MGH´ïÓdÓím7è|Îiƒ¥NŸ¶¦¬Ï[^‡<ŸSE	Àë¯SE?,x—ìuñW]øÅðKO×|FÐI©Å<–<PÌ#\n<ÇÉ?;rXŒž\0¯ø‚1ãÏÿ\0A;‘ÿ\0‘^Š+úÃÈÆÆ>1VIíÌú\\›øµ=\0(Š+÷óë\0\n\0¢Š\0\0 \n(¤\0\0QE\0\0WÓŸ³ÿ\0ŠPöÔä?ù\n*(¯Ì<Gÿ\0‘¿ÅÍž6mþêýQñoíAû]üQÐ¾6x—BÐ¼Bt\r3GœØÅ\r„+ûÐ¤Ÿ2Bû²çpUàrIEò9V_ƒžŒ¥F-¸¯²¿ÈùXÅYhÿÙ',	'max1',	NULL);

DROP TABLE IF EXISTS `message`;
CREATE TABLE `message` (
  `message_id` int(11) NOT NULL AUTO_INCREMENT,
  `login` varchar(50) NOT NULL,
  `dialog_id` int(11) NOT NULL,
  `time` int(11) NOT NULL,
  `text` text NOT NULL,
  PRIMARY KEY (`message_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO `message` (`message_id`, `login`, `dialog_id`, `time`, `text`) VALUES
(90,	'max1',	18,	1493469425,	'Hello'),
(91,	'ann',	18,	1493469426,	'Hi'),
(92,	'max1',	18,	1493469481,	'What do you do?'),
(93,	'ann',	18,	1493469482,	'I am sleeping'),
(94,	'max1',	18,	1493469565,	'Are you kidding me? Its a middle of the day now'),
(95,	'ann',	18,	1493469566,	'I felt asleep yesterday too late'),
(96,	'max1',	18,	1493469630,	'Ok'),
(97,	'max1',	18,	1493469671,	'May be you want to go with me in cafe? Eat some pizza'),
(98,	'ann',	18,	1493469672,	'Good idea. Will wait for you at my home in 1 hour'),
(99,	'max1',	18,	1493469811,	'Ok'),
(100,	'max1',	19,	1493469843,	'Hi. How are you?'),
(101,	'max1',	20,	1493469874,	'Go drink at the evening?'),
(102,	'max1',	22,	1493469913,	'I will do my best'),
(103,	'max1',	21,	1493469955,	'yo brother'),
(104,	'55555five55555',	21,	1493469956,	'bro'),
(105,	'max1',	42,	1493561094,	'test message'),
(106,	'ann',	42,	1493561096,	'test message 2'),
(107,	'max1',	42,	1493571856,	'Message 9450339369'),
(108,	'max1',	42,	1493575631,	'Lol'),
(109,	'max1',	45,	1493582571,	'Message 0165843749');

DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `login` varchar(50) NOT NULL,
  `pass` varchar(50) NOT NULL,
  `name` varchar(100) NOT NULL,
  `info` varchar(1000) NOT NULL,
  PRIMARY KEY (`login`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO `user` (`login`, `pass`, `name`, `info`) VALUES
('0506252963',	'5849a8357dfeefd36536165f0a799511',	'0515562777',	'0519902823'),
('4540941655',	'6b05e9bbefd267653843adde47a4d2ef',	'4549916970',	'4554347333'),
('4ex4q',	'f7177163c833dff4b38fc8d2872f1ec6',	'Olga L.',	''),
('55555five55555',	'c5fe25896e49ddfe996db7508cf00534',	'Maxim K.',	'Cool man'),
('ann',	'c4ca4238a0b923820dcc509a6f75849b',	'Ann K.',	'Cool girl'),
('bombyou',	'594f803b380a41396ed63dca39503542',	'Ahmed M.',	''),
('coolgirl',	'b53b3a3d6ab90ce0268229151c9bde11',	'Nasty A.',	''),
('ivan123',	'202cb962ac59075b964b07152d234b70',	'Ivan I.',	''),
('max1',	'698d51a19d8a121ce581499d7b701668',	'Max K.',	'Student, like sport and music'),
('monster77',	'698d51a19d8a121ce581499d7b701668',	'Oleg P.',	''),
('noname15',	'28dd2c7955ce926456240b2ff0100bde',	'Noname',	'Cool man'),
('tcar54',	'45c48cce2e2d7fbdea1afc51c7c6ad26',	'Petr S.',	'Cool man');

DROP TABLE IF EXISTS `user_dialog`;
CREATE TABLE `user_dialog` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `dialog_id` int(11) NOT NULL,
  `login` varchar(50) NOT NULL,
  `new` bit(1) NOT NULL DEFAULT b'1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO `user_dialog` (`id`, `dialog_id`, `login`, `new`) VALUES
(29,	18,	'max1',	CONV('0', 2, 10) + 0),
(30,	18,	'ann',	CONV('1', 2, 10) + 0),
(31,	19,	'max1',	CONV('0', 2, 10) + 0),
(32,	19,	'ivan123',	CONV('1', 2, 10) + 0),
(33,	20,	'max1',	CONV('0', 2, 10) + 0),
(34,	20,	'monster77',	CONV('1', 2, 10) + 0),
(35,	21,	'max1',	CONV('0', 2, 10) + 0),
(36,	21,	'55555five55555',	CONV('1', 2, 10) + 0),
(37,	22,	'max1',	CONV('0', 2, 10) + 0),
(38,	22,	'tcar54',	CONV('1', 2, 10) + 0),
(40,	23,	'ann',	CONV('1', 2, 10) + 0),
(42,	24,	'ann',	CONV('1', 2, 10) + 0),
(44,	25,	'ann',	CONV('1', 2, 10) + 0),
(46,	26,	'ann',	CONV('1', 2, 10) + 0),
(48,	27,	'ann',	CONV('1', 2, 10) + 0),
(50,	28,	'ann',	CONV('1', 2, 10) + 0),
(52,	29,	'ann',	CONV('1', 2, 10) + 0),
(54,	30,	'ann',	CONV('1', 2, 10) + 0),
(56,	31,	'ann',	CONV('1', 2, 10) + 0),
(58,	32,	'ann',	CONV('1', 2, 10) + 0),
(60,	33,	'ann',	CONV('1', 2, 10) + 0),
(62,	34,	'ann',	CONV('1', 2, 10) + 0),
(64,	35,	'ann',	CONV('1', 2, 10) + 0),
(66,	36,	'ann',	CONV('1', 2, 10) + 0),
(68,	37,	'ann',	CONV('1', 2, 10) + 0),
(70,	38,	'ann',	CONV('1', 2, 10) + 0),
(72,	39,	'ann',	CONV('1', 2, 10) + 0),
(74,	40,	'ann',	CONV('1', 2, 10) + 0),
(76,	41,	'ann',	CONV('1', 2, 10) + 0),
(77,	42,	'max1',	CONV('0', 2, 10) + 0),
(78,	42,	'ann',	CONV('1', 2, 10) + 0),
(80,	43,	'ann',	CONV('1', 2, 10) + 0),
(82,	44,	'ann',	CONV('1', 2, 10) + 0),
(84,	45,	'ann',	CONV('1', 2, 10) + 0),
(86,	46,	'ann',	CONV('1', 2, 10) + 0),
(88,	47,	'ann',	CONV('1', 2, 10) + 0);

-- 2017-04-30 22:46:46
