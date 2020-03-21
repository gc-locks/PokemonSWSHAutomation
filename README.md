# PokemonSWSHAutomation

PokemonSWSHAutomation はswitch製のコントローラをエミュレーションしたarduinoをPCから操作するソフトです。

[この記事](https://qiita.com/chibi314/items/975784f6e951341fc6ce) にあるコードを元に作成されています。

このコードはUSBではなくEthernetを用いて通信します。
そのためEthernet Shield 2などが必要になります。

## arduino へファームウェアのインストール

PokemonSWSHAutomation\arduino_firmware\ethernet_server ディレクトリで

1. config.sample.h をコピーして config.h にする
1. config.hを編集して `mac` にArduinoのethernetのMACアドレスを入れ、`ip` に固定IPアドレスを入れる。ただしPCからアクセスできるネットワークであること
1. ethernet_server.ino をArduino IDEからインストールする
