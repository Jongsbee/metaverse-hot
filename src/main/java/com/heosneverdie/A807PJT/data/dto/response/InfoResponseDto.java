package com.heosneverdie.A807PJT.data.dto.response;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Getter
@NoArgsConstructor
@Builder
@AllArgsConstructor
public class InfoResponseDto {

    private Integer exp;
    private Integer coin;
    private String nickname;
    private Boolean isHammerUnlocked, isDeveloperUnlocked,isAlyakOK;
}
