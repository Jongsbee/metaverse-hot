package com.heosneverdie.A807PJT.data.dto.request;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Getter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class RequestExpCoinDto {
    private Integer exp;
    private Integer coin;
    private String nickname;
}


